const { data } = require("jquery");

function saveNotesData() {
	var notes = document.getElementById("notes").value;

	var data = {
		notes: notes
	};
	var action = "SaveNotesData";

	sendAjax(action, data);
}

function saveTrainigDaysData(day) {

	var data = {
		day: day
	};
	var action = "SaveTrainigDaysData";

	sendAjax(action, data);
}

function saveTitleData() {
	var title = document.getElementById("title").value;

	var data = {
		title: title
	};
	var action = "SaveTitleData";

	sendAjax(action, data);
}

function saveRepsAndWeightsData(id, position) {
	event.preventDefault();

	var reps = document.getElementById(position + "-r-" + id).value;
	var weights = document.getElementById(position + "-w-" + id).value;

	var data = {
		values: reps + ";" + weights,
		exerciseId: id,
		seriesPosition: position
	};

	var action = "SaveRepsAndWeightsData";

	sendAjax(action, data);
}

function sendAjax(action, data) {
	$.ajax({
		type: 'GET',
		url: '/Visitor/TrainingPlan/' + action,
		data: data,
		success: function (response) {
			console.log("Dane zapisane do sesji.");
		},
		error: function (response) {
			console.log("Wystąpił błąd podczas zapisywania danych do sesji.");
		}
	});
}