function loadContent(type) {
    $("#loader").show();
    $.ajax({
        url: '/Visitor/ExercisesSet/GetDynamicContent',
        type: 'GET',
        data: { exerciseType: type },
        success: function (response) {
            if (response.length > 0) {
                var array = response.split(",");
                var result = document.createElement("div");
                result.classList.add("row");
                for (const el of array) {

                    var subArray = el.split("&");

                    var card = document.createElement("div");
                    card.classList.add("card", "m-3", "p-0", "showButton");
                    card.style.width = "300px";
                    card.style.height = "255.6px";

                    var cardHeader = document.createElement("div");
                    cardHeader.classList.add("card-header", "p-0");
                    cardHeader.id = "cardHeader" + subArray[2];


                    var cardBtn = document.createElement("a");
                    cardBtn.classList.add("btn", "cardBtn");
                    cardBtn.style.borderRadius = "50px";
                    cardBtn.style.backgroundColor = "#33C26F";
                    cardBtn.href = "/Visitor/TrainingPlan/AddExerciseId/" + subArray[2]
                    cardBtn.textContent = "Dodaj";
                    

                    var cardImg = document.createElement("img");
                    cardImg.src = "\\" + subArray[1];
                    cardImg.alt = "Zdjęcie ćwiczenia";
                    cardImg.style.width = "300px";
                    cardImg.style.height = "200.81px";
                    cardImg.style.overflow = "hidden";

                    cardHeader.appendChild(cardImg);


                    var cardBody = document.createElement("div");
                    cardBody.classList.add("card-body");
                    cardBody.style.height = "95%";

                    var cardTitle = document.createElement("h6");
                    cardTitle.color = "green";
                    cardTitle.classList.add("card-title");
                    cardTitle.style.overflow = "hidden";
                    cardTitle.style.whiteSpace = "nowrap";

                    var title = document.createElement("a");
                    title.href = "../Visitor/ExercisesSet/Exercise/" + subArray[2];
                    title.text = subArray[0];
                    title.style.textDecoration = "none";
                    title.style.color = "#33C26F";
                    title.color = "green";

                    cardTitle.appendChild(title);

                    cardBody.appendChild(cardTitle);
                    cardBody.appendChild(cardBtn);

                    card.appendChild(cardHeader);
                    card.appendChild(cardBody);


                    result.appendChild(card);
                }
                $('#loader').hide();
                $('#displayContent').html(result);
            }
            else {
                $('#loader').hide();
                $('#displayContent').html('Brak danych');
            }

        },
        error: function () {
            alert('Wystąpił błąd pobierania zawartości z bazy danych.');
        }
    })
}


// $(document).ready(function () {
// 	// $(".showButton").mouseenter(function () {
// 	// 	$(this).find(".addButton").show();
// 	// }).mouseleave(function () {
// 	// 	$(this).find(".addButton").hide();
// 	// });

// $("#testDiv").mouseenter(function () {
// 	$("#cardBtn1350").show();
// }).mouseleave(function () {
// 	$("#cardBtn1350").hide();
// })
// });