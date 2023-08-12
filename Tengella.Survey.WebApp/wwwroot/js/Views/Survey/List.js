$(function () {
	var dataTable = $('#survey-table').DataTable({
		language: {
			searchPlaceholder: "Search",
			search: "" // Placeholder instead of label
		},
		columnDefs: [
			{
				targets: -1,
				orderable: false, //Disable sorting for last column
				width: '7%'
			},
			{
				targets: -2,
				width: '10%'
			},
		]
	});

	// Enable Bootstrap tooltips
	$(function () {
		$('[data-toggle="tooltip"]').tooltip();
	});

	// Attach event to delete links
	$(".delete-link").on("click", function () {
		var surveyId = $(this).attr("value");
		var form = $("#deleteForm-" + surveyId);
		if (form && confirm('Are you sure you want to delete this survey? This action cannot be undone.')) {
			form.submit();
		}
	});
});