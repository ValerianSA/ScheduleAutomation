/*const { default: update } = require("./src/methods/update");*/

$(function () {




    var instructorRoleBarValue = $('#instructorRoleBar').text($(this).data('instructorRoleBar'));
    var sessionStatusBarValue = $('#box').val($(this).data('sessionstatusid'));


    switch (sessionStatusBarValue) {
        case "1":
            $('#sessionStatusBar').css('background-color', 'red');
            break;
        case "2":
            $('#sessionStatusBar').css('background-color', 'yellow');
            break;
        case "3":
            $('#sessionStatusBar').css('background-color', 'green');
            break;
        default:
            $('#sessionStatusBar').css('background-color', 'blue');
            console.log(sessionStatusBarValue);
            break;
    }

    switch (instructorRoleBarValue) {
        case "Instructor":
            $('#instructorRoleBar').css('background-color', 'purple');
            break;
        case "Observer":
            $('#instructorRoleBar').css('background-color', 'yellow');
            break;
        default:
            $('#instructorRoleBar').css('background-color', 'blue');
            console.log(instructorRoleBarValue);
            break;
    }

    //if (sessionStatusBarValue === 1) {
    //    $('#sessionStatusBar').css('background-color', 'red');
    //}
    //else if (sessionStatusBarValue === 2) {
    //    $('#sessionStatusBar').css('background-color', 'yellow');
    //}
    //else {
    //    $('#sessionStatusBar').css('background-color', 'blue');
    //}
});






function getCourseList() {

    $.ajax({
        url: "/Calendar/GetSession",
        method: "GET",
        success: function (data) {

            var inData = JSON.parse(data);
            var courseDefault = '<option value="0">Choose a session name</option>';
            //Display course names as a list on modal
            for (var i = 0; i < inData.length; i++) {
                if (inData[i].CourseName != "null") {
                    courseDefault += '<option value="' + inData[i].CourseID + '">' + inData[i].CourseName + '</option>';
                }
            }
            $("#courseList").html(courseDefault);
        },
        error: function (err) {
            console.log(err);
        }
    })

}


//Get list of instructors for AddSession form
function getInstructorList() {

    $.ajax({
        url: "/Calendar/GetInstructors",
        method: "GET",
        success: function (data) {

            var inData = JSON.parse(data);
            var instructorDefault = '<option value="0">Choose an instructor</option>';
            //Display course names as a list on modal
            for (var i = 0; i < inData.length; i++) {
                if (inData[i].Username != "null") {
                    instructorDefault += '<option data-instructorid="' + inData[i].Username + '" value="' + inData[i].Username + '">' + inData[i].FirstName + " " + inData[i].LastName + '</option>';
                }
            }
            $("#instructorList").html(instructorDefault);
        },
        error: function (err) {
            console.log(err);
        }
    })

}



//$('#addSessionSymbol').click(function () {

//    $.ajax({
//        url: "/Calendar/GetSession",
//        method: "GET",
//        success: function (data) {

//            var inData = JSON.parse(data);
//            var courseDefault = '<option value="-1">Choose a session name</option>';
//            //just for testing
//            console.log(inData);
//            //Display course names as a list on modal
//            for (var i = 0; i < inData.length; i++) {
//                if (inData[i].CourseName != "null") {
//                    courseDefault += '<option value="' + inData[i].CourseID + '">' + inData[i].CourseName + '</option>';
//                }
//            }
//            $("#courseList").html(courseDefault);
//        },
//        error: function (err) {
//            console.log(err);
//        }
//    })
//})


//function addSessionModalValidations() {



//    if (document.getElementById('#courseList').nodeValue == null) {
//        //show the session saved notification
//        $('.sessionAddFieldNotification').fadeIn('fast');
//        //autohide the session saved notification
//        setTimeout(function () {
//            $('.sessionAddFieldNotification').fadeOut('fast');
//        }, 5000);
//        document.getElementById('#courseList').style.bordercolor = red;
//    }

//    if (document.getElementById('#sessionCode').value == "") {
//        //show the error message
//        $('.sessionAddFieldNotification').fadeIn('fast');
//        //autohide the error message
//        setTimeout(function () {
//            $('.sessionAddFieldNotification').fadeOut('fast');
//        }, 5000);
//        document.getElementById('#sessionCode').style.bordercolor = red;
//    }

//    if (document.getElementById('#instructor').value == "") {
//        //show the session saved notification
//        $('.sessionAddFieldNotification').fadeIn('fast');
//        //autohide the session saved notification
//        setTimeout(function () {
//            $('.sessionAddFieldNotification').fadeOut('fast');
//        }, 5000);
//        document.getElementById('#instructor').style.bordercolor = red;
//    }


//    else {
//        retrieveModalFormData();
//    }


//}


function AddSession() {


    $('#CalModal').show();

    var addSession = new Object();
    addSession.CourseID = $('#courseList').find(':selected').val();
    addSession.sessionCode = $('#sessionCode').val();
    addSession.Instructor = $('#instructorList').text();
    addSession.TypeOfParticipation = $('#TypeOfParticipant').val();
    addSession.CourseName = $('#courseList').find(':selected').text();
    addSession.StartTime = $('#startTime').val();
    addSession.empID = $('#instructorList').data("instructorid");
    addSession.EndTime = $('#endTime').val();
    
        $.ajax({

            type: "POST",
            url: "/Calendar/addSession",
            contentType: 'application/json',
            data: JSON.stringify(addSession),
            success: function (response) {
                Console.log('Session saved!');
            }
        })

        //Hide error message
        $('.sessionAddFieldNotification').fadeOut('fast');
        //show the session saved notification
        $('.sessionAddNotification').fadeIn('fast');
        //autohide the session saved notification
        setTimeout(function () {
            $('.sessionAddNotification').fadeOut('fast');
        }, 5000);

        //clears form for new session addition
        //$('#courseList').prop('selectedIndex', 0);
        //$('#courseList').val("");
        //$('#sessionCode').val("");
        //$('#TypeOfParticipant').prop('selectedIndex', 0);
}



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



/* Set the width of the side navigation to 230px */
function openEditSidebar() {
    var e = document.getElementById("editingSidebar");
    var f = document.getElementById("editingSidebarOpen");
    var width = '400px';

    e.style.width = width;
        e.style.transition = '0.2s';
        f.style.backgroundColor = "white";
        f.style.color = "#154a94";
        f.style.transition = "0.5s";
}

function closeEditSidebar() {
    var e = document.getElementById("editingSidebar");
    var f = document.getElementById("editingSidebarOpen");

    if (e.style.width === openEditSidebar.width) {
        e.style.width = '0px';
        e.style.transition = '0.2s';
        f.style.backgroundColor = "transparent";
        f.style.color = "white";
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





//Get the list of sessions for a specific cell
function getCellSessionList(empID, date) {


    var cellSessionDetails = new Object();
    cellSessionDetails.empID = empID;
    cellSessionDetails.startTime = date;
    $.ajax({
        url: "/Calendar/GetCellSessionList",
        type: "POST",
        contentType: 'application/json',
        data: JSON.stringify(cellSessionDetails),
        success: function (data) {

            var inData = JSON.parse(data);
            var sessionDefault = '';
            //just for testing
            //console.log(inData);
            //Display course names as a list on modal
            for (var i = 0; i < inData.length; i++) {
                var startDate = moment(inData[i].startTime).format('MMM Do YY, h:mm a');
                var endDate = moment(inData[i].endTime).format('MMM Do YY, h:mm a');
                if (inData[i].sessionName != "null") {
                    sessionDefault += '<tr> <td class="sessionCodeValue">' + inData[i].sessionName + '</td> <td class="sessionStartTimeValue">' + startDate + '</td> <td class="sessionEndTimeValue">' + endDate + '</td> <td><button type="button" class="btn btn-primary viewsessionsbtnedit" data-dismiss="static" data-editsessionid="' + inData[i].sessionID + '" data-sessioncodevalue="' + inData[i].sessionCode + '" data-sessionname="' + inData[i].sessionName + '" data-starttime="' + inData[i].startTime + '" data-endtime="' + inData[i].endTime + '">Edit</button></td></tr>';
                }
                else {
                    sessionDefault += '<tr colspan="3"><td>There are no sessions available for this day</td> </tr>';
                }
            }
            $("#cellSessions").html(sessionDefault);
        },
        error: function (err) {
            sessionDefault += '<tr colspan="3"><td>There are no sessions available for this day</td> </tr>';
            $("#cellSessions").html(sessionDefault);
        }
    })

}

$(document).on("click", ".viewsessionsbtnedit", function () {
    $('.editSessionCode').val($(this).data('sessioncodevalue'));
    $('.editSessionName').val($(this).data('sessionname'));
    $('.editStartTime').val($(this).data('starttime'));
    $('.editEndTime').val($(this).data('endtime'));
    $('.btnSaveEdits').attr('data-sessionid', $(this).data('editsessionid'));
});


//function fillSidebarValuesWithEditdata() {
    //sessionID, sessionCode, sessionName, startTime, endTime, instructor
    //$('.editSessionCode').val(sessionCode);
    //$('.editSessionName').val(sessionName);
    //$('.editStartTime').val(startTime);
    //$('.editEndTime').val(endTime);
    //$('.editInstructor').val(instructor);

    //console.log('in the edit button');
//}





//Edit a specific session
function UpdateSession() {

    var updatedSessionInfo = new Object();
    updatedSessionInfo.sessionID = $('.btnSaveEdits').data('sessionid');
    updatedSessionInfo.sessionCode = $('.editSessionCode').val();
    updatedSessionInfo.sessionName = $('.editSessionName').val();
    updatedSessionInfo.startTime = $('.editStartTime').val();
    updatedSessionInfo.endTime = $('.editEndTime').val();

    console.log(updatedSessionInfo);

    $.ajax({

        type: "POST",
        url: "/Calendar/updateSession",
        contentType: 'application/json',
        data: JSON.stringify(updatedSessionInfo),
        success: function (response) {
            //Hide success message
            $('.sessionEditNotification').fadeOut('fast');
            //show tdhe session edited notification
            $('.sessionEditNotification').fadeIn('fast');
            //autohide the session edited notification
            setTimeout(function () {
                $('.sessionEditNotification').fadeOut('fast');
            }, 5000);
        }
    })

}


/*Administrator Views scripts*/

function getPendingSessionList() {

    $.ajax({
        url: "/Admin/GetPendingSessionList",
        type: "GET",
        contentType: 'application/json',
        success: function (data) {

            var inData = JSON.parse(data);
            var sessionDefault = '';
            //just for testing
            console.log(inData);
            //Display course names as a list on modal
            for (var i = 0; i < inData.length; i++) {
                var startDate = moment(inData[i].startTime).format('MMMM Do YYYY, h:mm a');;
                var endDate = moment(inData[i].endTime).format('MMMM Do YYYY, h:mm a');
                if (inData[i].sessionName != "not null") {
                    sessionDefault += '<tr> <td>'+ inData[i].sessionCode +'</td> <td class="pendingsessionCodeValue">' + inData[i].sessionName + '</td> <td class="pendingsessionStartTimeValue">' + startDate + '</td> <td class="pendingsessionEndTimeValue">' + endDate + '</td> <td class="pendingsessionStatusValue">' + inData[i].sessionStatus + '</td> </td > <td class="pendingsessionStatusValue"><button type="button" class="btn pendingbtn" data-toggle="modal" data-target="#AdminsessionModal" data-dismiss="static" data-pendingsessionid="' + inData[i].sessionID + '" data-sessioncodevalue="' + inData[i].sessionCode + '" data-sessionname="' + inData[i].sessionName + '" data-starttime="' + inData[i].startTime + '" data-endtime="' + inData[i].endTime + '">Update</button></td></tr>';
                }
                else {
                    sessionDefault += '<tr colspan="4"><td>An Error has occurred, please contact your Admin for assistance</td></tr>';
                }
            }
            $("#pendingCellSessions").html(sessionDefault);
        },
        error: function (err) {
            //sessionDefault += '<tr colspan="3"><td>There are no sessions with pending changes</td> </tr>';
            sessionDefault = "error";
            $("#pendingCellSessions").html(sessionDefault);
        }
    })
}


function getCompletedSessionList() {

    $.ajax({
        url: "/Admin/GetCompletedSessionList",
        type: "GET",
        contentType: 'application/json',
        success: function (data) {

            var inData = JSON.parse(data);
            var sessionDefault = '';
            //just for testing
            console.log(inData);
            //Display course names as a list on modal
            for (var i = 0; i < inData.length; i++) {
                var startDate = moment(inData[i].startTime).format('MMMM Do YYYY, h:mm a');
                var endDate = moment(inData[i].endTime).format('MMMM Do YYYY, h:mm a');

                if (inData[i].sessionName != "null") {
                    sessionDefault += '<tr> <td>' + inData[i].sessionCode +'</td> <td class="completedsessionCodeValue"><button type="button" class="btn completebtn text-left" data-toggle="modal" data-target="#AdminsessionModal" data-dismiss="static" data-completesessionid="' + inData[i].sessionID + '" data-sessioncodevalue="' + inData[i].sessionCode + '" data-completesessionname="' + inData[i].sessionName + '" data-starttime="' + inData[i].startTime + '" data-endtime="' + inData[i].endTime + '">' + inData[i].sessionName + '</button></td> <td class="completedsessionStartTimeValue">' + startDate + '</td> <td class="completedsessionEndTimeValue">' + endDate + '</td> <td class="completedsessionStatusValue">' + inData[i].sessionStatus + '</td> <td></tr>';
                }
            }
            $("#completedCellSessions").html(sessionDefault);
        },
        error: function (err) {
            //sessionDefault += '<tr colspan="3"><td>There are no sessions with pending changes</td> </tr>';
            sessionDefault = "error";
            $("#completedCellSessions").html(sessionDefault);
        }
    })

}


function updateNewPendingSession() {

    var updatedSessionInfo = new Object();
    updatedSessionInfo.sessionID = $('.updatebtn').data('sessionid');
    //updatedSessionInfo.sessionCode = $('.editSessionCode').val();
    //updatedSessionInfo.sessionName = $('.editSessionName').val();
    //updatedSessionInfo.startTime = $('.editStartTime').val();
    //updatedSessionInfo.endTime = $('.editEndTime').val();

    $.ajax({
        type: "POST",
        url: "/Admin/updateSession",
        contentType: 'application/json',
        data: JSON.stringify(updatedSessionInfo),
        success: function (response) {
            alert('session updated successfully');
            var inData = JSON.parse(response);
            console.log(inData.sessionName);
            //Hide success message
            //     $('.sessionEditNotification').fadeOut('fast');
            //show tdhe session edited notification
            //     $('.sessionEditNotification').fadeIn('fast');
            //autohide the session edited notification
            //setTimeout(function () {
            //     $('.sessionUpdateNotification').fadeOut('fast');
            //     }, 5000);
        }
    })
}






