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
