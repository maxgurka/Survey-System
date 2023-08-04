
$(function () {
    var minFields = 2;; // Minimum number of answers allowed to a question
    var maxFields = 10; // Maximum number of answers allowed to a question

    // Disable/enable buttons depending on the amount of answer-fields created
    function updateButtonStates() {
        $('.form-control').removeClass('is-invalid');
        $('.question-container').each(function () {
            var textFieldCount = $(this).find('.answer').length;
            var addButton = $(this).find('.add-answer');
            var removeButtons = $(this).find('.remove-answer');

            addButton.prop('disabled', textFieldCount >= maxFields);
            removeButtons.prop('disabled', textFieldCount <= minFields);
        });
    };

    // Returns html for a new answer alternative input
    function createAnswerTextField() {
        return `
            <div class="answer input-group mb-1">
                <input type="text" class="form-control" placeholder="Alternative">
                <button type="button" class="remove-answer btn btn-sm accent-theme-secondary"><span class="bi-dash-square"></span></button>
            </div>`;
    }

    // Returns html for a new question input
    function createQuestionTextField() {
        return `
            <div class="input-group mb-3">
                <input type="text" class="question-name form-control" placeholder="Question">
                <button type="button" class="remove-question btn btn-sm accent-theme-secondary"><span class="bi-dash-square"></span></button>
            </div>`;
    }

    //Check the user input before submitting
    function validateForm() {

        valid = true;

        // Reset previous styling on all fields
        $('.form-control').removeClass('is-invalid');

        // Check survey name and description
        const surveyName = $('#survey-name');
        const surveyDescription = $('#survey-desc');

        if (!surveyName.val().trim()) {
            surveyName.addClass('is-invalid');
            valid = false;
        }

        if (!surveyDescription.val().trim()) {
            surveyDescription.addClass('is-invalid');
            valid = false;
        }

        // Check each question
        $('.question-container').each(function () {
            const questionName = $(this).find('.question-name');
            const answers = $(this).find('.answer input');

            // Check question name
            if (!questionName.val().trim()) {
                questionName.addClass('is-invalid');
                valid = false;
            }

            // Check each answer
            answers.each(function () {
                const answer = $(this);

                if (!answer.val().trim()) {
                    answer.addClass('is-invalid');
                    valid = false;
                }
            });
        });

        return valid;
    }

    // Enable sorting on the question-container
    $("#content-container").sortable({
        axix: "y"
    });

    $(".content-container").sortable({
        handle: ".question-drag-handle",
        axis: "y",
        containment: "#content-container",
    });

    // Serialize the form data as a JSON object
    function serializeFormData() {
        var surveyName = $('#survey-name').val();
        var surveyDescription = $('#survey-desc').val();
        var questions = [];

        $('.question-container').each(function () {
            var questionName = $(this).find('.question-name').val();
            var answers = [];

            $(this).find('.answer input').each(function () {
                answers.push({ text: $(this).val() });
            });

            questions.push({
                name: questionName,
                answers: answers
            });
        });

        // Get the date from the date picker
        var endDate = $('#date-picker').val();

        // Create the JSON object
        var data = {
            name: surveyName,
            description: surveyDescription,
            questions: questions
        };

        // Add the end date to the JSON object if it exists
        if (endDate) {
            data.endDate = endDate;
        }

        return JSON.stringify(data);
    }

    // Handle form submission
    $('#submit-survey').click(async function () {

        // Validate the form before submitting
        if (!validateForm()) {
            return;
        }

        try {
            const response = await fetch("Create", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: serializeFormData(),
            });
            if (response.ok) {
                const result = await response;
                window.location.href = response.url;
                console.log(response.body);
                console.log("Success:", result);
            }
            else {
                // TODO: Handle errors when the response status is not ok
                console.error("Error:", response.statusText);
            }
        } catch (error) {
            // Handle other errors
            console.error("Error:", error);
        }
    });

    // Add multiple-choice question
    $('#add-mc-question').click(function () {
        const questionHtml = `
            <div class="question-container border background-theme-secondary question-drag-handle">
                ${createQuestionTextField()}
                ${Array.from({ length: minFields }, createAnswerTextField).join('')}
                <button type="button" class="add-answer btn btn-sm accent-theme-secondary mb-3"><span class="bi-plus-square"></span></button>
            </div>`;

        $('#content-container').append(questionHtml);
        updateButtonStates();
    });

    // Add free-text question
    $('#add-question').click(function () {
        const html = `
            <div class="question-container border background-theme-secondary question-drag-handle">
                ${createQuestionTextField()}
            </div>`;
        $('#content-container').append(html);
        updateButtonStates();
    });

    //Remove answer alternative
    $('#content-container').on('click', '.remove-answer', function () {
        $(this).closest('.answer').remove();
        updateButtonStates();
    });

    // Add answer alternative
    $('#content-container').on('click', '.add-answer', function () {
        $(this).before(createAnswerTextField());
        updateButtonStates();
    });

    //Remove question
    $('#content-container').on('click', '.remove-question', function () {
        $(this).closest('.question-container').remove();
        updateButtonStates();
    });
    updateButtonStates();
});