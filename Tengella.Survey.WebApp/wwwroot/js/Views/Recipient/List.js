var anyCheckboxChecked = false;
var selectedDropdownValue;

var dataTable = $('#recipient-table').DataTable({
	language: {
		searchPlaceholder: "Search",
		search: "" // Placeholder instead of label
	},
	columnDefs: [
		{
			targets: -1,
			orderable: false, //Disable sorting for last column
			className: 'dt-body-center',
			width: '1' // Small width makes the column width fit content
		},
		{
			targets: 0,
			orderable: false, //Disable sorting for first column
			className: 'dt-body-center',
			width: '1'
		}
	],
	order: [[1, 'asc']]
});

// Checkbox event handler
$('.rowCheckbox').on('change', function updateDropdownButtonState() {
	anyCheckboxChecked = $('.rowCheckbox:checked').length > 0;

	if (anyCheckboxChecked) {
		$('#clearButton').removeClass('btn-primary').addClass('btn-secondary').text('Add');
	} else {
		$('#clearButton').removeClass('btn-secondary').addClass('btn-primary').text('Clear');
	}
});

// Prevent form submission if input is empty
$("#createListForm").on("submit", function (event) {
	if ($("input[name='listName']").val().trim() === "") {
		event.preventDefault();
	}
});

// Choosing a list from the dropdown
$("#recipientListDropdown").on("change", function () {

	selectedDropdownValue = $(this).val();

	if ($(this).val() !== "") {
		if (!anyCheckboxChecked) {
			// Redirect to the List action with the selected ID in the URL
			window.location.href = "/Recipient/List/" + $(this).val();
		}
	}
});

// Get ids of the recipients whose checkboxes has been checked
function getSelectedRecipientIds() {
	var selectedIds = [];
	$('.rowCheckbox:checked').each(function () {
		selectedIds.push($(this).val());
	});
	return selectedIds;
}

// Clear button
$("#clearButton").on("click", function () {

	if (anyCheckboxChecked) {
		var listId = parseInt(selectedDropdownValue);
		var recipientIds = getSelectedRecipientIds();
		console.log(selectedDropdownValue);
		console.log(listId);

		// Use the listId and selectedRecipientIds to make an AJAX request
		$.ajax({
			url: "/Recipient/AddToList",
			type: "POST",
			data: {
				listId: listId,
				recipientIds: recipientIds
			},
			success: function (data) {
				console.log(data);
				// Handle the server response if needed
				// For example, you could refresh the page or display a success message
			},
			error: function (error) {
				// Handle the error if needed
			}
		});
	}

	// Redirect to the List action without any ID
	window.location.href = "/Recipient/List";
});