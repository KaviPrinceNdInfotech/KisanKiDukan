/// <reference path="jquery-2.1.4.min.js" />
var txtTotalItems = $("#cartCount");
var txtTotalPrice = $("#cartTotal");

loadCart();
function loadCart()
{
    $.ajax({
        url: `/Kart/LoadCart`,
        type: 'get',
        success: function (response) {
            txtTotalItems.text(response.NOI);
            txtTotalPrice.text('Rs. '+response.Total);
            window.location.href = "#shoppingCart";
        }
    });
}





function addToCart(obj) {
   
    if (document.cookie.indexOf('user') < 0) {
        debugger
        var url = window.location.href;
        window.location.href = '/Home/Login?url=' + url;
        return;
    }
   //debugger

   let currentElem = $(obj);
    //debugger
    var prodDiv = currentElem.parents("#productDiv");
    var ddVariants = prodDiv.find('#variants');
    var weight = ddVariants.find('option:selected').attr("weight");
    var product_id = ddVariants.find('option:selected').attr("productId");
    var metric_id = ddVariants.val();
    //var vendorId = currentElem.find('option:selected').attr("VendorId");
    //&vendorId=${vendorId}

    if (weight == undefined || weight == null || product_id == undefined || product_id == null || metric_id == undefined || metric_id == null)
        return;
    
    $.ajax({
        
        url: `/Kart/AddToCart?productId=${product_id}&metricId=${metric_id}&weight=${weight}`,
        type: 'get',
        success: function (response) {
            txtTotalItems.text(response.NOI);
            txtTotalPrice.text('Rs. ' + response.Total);
            window.location.href = "#shoppingCart";
        }
    });
}


//function addToCart(obj) {
//    debugger
//    if (document.cookie.indexOf('user') < 0) {
//        var url = window.location.href;
//        window.location.href = '/Home/Login?url=' + url;
//        return;
//    }
    
//    let currentElem = $(obj);
//    var prodDiv = currentElem.parents("#productDiv");
//    //var ddVariants = prodDiv.find('#variants');
//    //var weight = ddVariants.find('option:selected').attr("weight");
//    var product_id = ddVariants.find('option:selected').attr("productId");
//    //var metric_id = ddVariants.val();

//    //if (weight == undefined || weight == null || product_id == undefined || product_id == null || metric_id == undefined || metric_id == null)
//    //    return;

//    $.ajax({
//        // url: `/Kart/AddToCart?productId=${product_id}&metricId=${metric_id}&weight=${weight}`,
//        url: `/Kart/AddToCart?productId=${product_id}`,
//        type: 'get',
//        success: function (response) {
//            txtTotalItems.text(response.NOI);
//            txtTotalPrice.text('Rs. ' + response.Total);
//            window.location.href = "#shoppingCart";
//        }
//    });
//}

function prodAvailable(obj)
{

    let currentElem = $(obj);
    var isAvailable = parseInt(currentElem.find('option:selected').attr("isAvail"));
    var avStatus = currentElem.parent().parent().find("#availStatus");
    var cartButton = currentElem.parent().parent().find('#btnCart');
     
    if (isAvailable == 1)
    {
        avStatus.text("In stock").addClass("text-success").removeClass('text-danger');;
        cartButton.css("pointer-events", "auto");
    }
    else
        {
        avStatus.text("Out of-stock").addClass("text-danger").removeClass('text-success');;
        cartButton.css("pointer-events","none" );
    }
    
}