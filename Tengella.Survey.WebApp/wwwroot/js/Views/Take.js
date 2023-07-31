$(document).ready(function () {

    // Function to serialize the form data as a JSON object
    function serializeFormData() {

        var answers = [];
        $('.question-container').each(function () {
            var answer = {};

            if ($(this).find('input[type="radio"]').length > 0) {
                // Multiple-choice question
                var selectedAnswerId = $(this).find('input[type="radio"]:checked').val();
                if (selectedAnswerId) {
                    // Get the question ID from the 'name' attribute of the checked radio input
                    var questionId = $(this).find('input[type="radio"]:checked').attr('name');
                    answer = {
                        questionId: questionId,
                        content: selectedAnswerId
                    };
                    answers.push(answer);
                }
            } else {
                // Free-text question
                var freeTextAnswer = $(this).find('textarea').val().trim();
                if (freeTextAnswer !== "") {
                    // Get the question ID from the 'name' attribute of the textarea
                    var questionId = $(this).find('textarea').attr('name');
                    answer = {
                        questionId: questionId,
                        content: freeTextAnswer
                    };
                    answers.push(answer);
                }
            }
        });

        var jsonData = {
            surveyId: surveyId,
        answers: answers
    };

    return JSON.stringify(jsonData);
}

		// Handle form submission
		$('#submit-survey').click(async function postJSON() {

    // TODO: Make sure survey is filled in correctly

    try {
        const response = await fetch("Take", {
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
            // Handle errors when the response status is not ok
            console.error("Error:", response.statusText);
        }
    } catch (error) {
        // Handle other errors
        console.error("Error:", error);
    }
});
});