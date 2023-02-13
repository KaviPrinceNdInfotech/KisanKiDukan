/// <reference path="Library/jquery-1.7.1.min.js" />

$("#State_Id").change(function () {
    var stateId = $(this).val();
    var City = $("#City_Id");
    City.empty();
    City.append("<option value=''>Select City</option>");
    $.ajax({
        url: '/Home/GetCitiesByState?stateId=' + stateId,
        type: 'get',
        success: function (r) {
            $.each(r, function (Key, val) {
                City.append("<option value='" + val.City_Id + "'>" + val.CityName + "</option>");

            });

        },
        error: function (e) {
            console.log(e.responseText)
        }
    })
})

$("#btnFormSubmit").click(function () {
    var frm = $("#frmUserReg");
    var frmData = frm.serialize();
    var msg = $("#msg");
    msg.text("wait..").css('color', 'black');
    $.ajax({
        url: '/home/userregister',
        type: 'post',
        data: frmData,
        success: function (response) {
            if(response=="ok")
            {
                msg.text("Saved Successfully").css("color", "green");
                $("input").val('');
            }
            else
                msg.text(response).css("color", "red");

        },
        error: function (e) {
            console.log(e.responseText);
            msg.text("Error occured").css("color", "red");
        }
    });
});