$(function () {

	// Edit button
	$("#editButton").on("click", function () {
		$("input").prop("disabled", false);
		$("select").prop("disabled", false);
		$("#editButton").hide();
		$("#saveButton").show();
		$("#cancelButton").show();
		$("#deleteButton").show();
	});

	// Enable the Save button when user makes changes to inputs or dropdown
	$("input, select").on("input change", function () {
		$("#saveButton").prop("disabled", false);
	});

	// Save button
	$("#saveButton").on("click", function (e) {

		// Show a confirmation popup before saving
		var confirmSave = window.confirm("Save changes?");

		if (!confirmSave) {
			e.preventDefault();
		}
	});

	// Cancel button
	$("#cancelButton").on("click", function () {

		// Show a confirmation window before discarding, but only if any changes has been made
		var cancel = true;
		if (!$("#saveButton").prop("disabled")) {
			cancel = window.confirm("Discard changes?");
		}

		if (cancel) {
			// Cancel changes by simply refreshing the page
			window.location.reload();
		}
	});
	// Delete button
	$("#deleteButton").on("click", function () {

		// Show a confirmation window before deleting
		var del = window.confirm("Delete recipient from database?");

		if (del) {
			$("form").attr("action", "/Recipient/Delete").submit();
		}
	});
});