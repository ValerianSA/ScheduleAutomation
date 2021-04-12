


/* Set the width of the side navigation to 230px */
function openNav() {
    var e = document.getElementById("mySidebar");
    var f = document.getElementById("SidebarOpen");
    if (e.style.width === '200px') {
        e.style.width = '0px';
        e.style.transition = '0.2s';
        f.style.backgroundColor = "transparent";
        f.style.color = "white";
    }
    else {
        e.style.width = '200px';
        e.style.transition = '0.2s';
        f.style.backgroundColor = "white";
        f.style.color = "#154a94";
        f.style.transition = "0.5s";
    }
}

/*make Plus sign visible when div is hovered over (onmouseover)*/
//function showPlus() {
//    document.getElementById("plusSign").style.visibility = "visible";
//}


/*make Plus sign hidden when div is NOT hovered over (onmouseout)*/
//function hidePlus() {
//    document.getElementById("plusSign").style.visibility = "hidden";
//}






function getInstructorSessions() {

	$.ajax({
		url: "/Calendar/GetSession",
		method: "GET",
        success: function (data) {

            data = JSON.parse(data);
            console.log(data);
		},
		error: function (err) {
			console.log(err);
        }
	})

}




