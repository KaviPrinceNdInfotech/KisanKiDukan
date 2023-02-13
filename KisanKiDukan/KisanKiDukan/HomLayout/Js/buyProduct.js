   
//check box cheked

    $("#all").click(function () {
        var allProduct = $(".chkAvail");
        var chkCurrent = $(this);
        if (chkCurrent.prop("checked") == true) {
            allProduct.prop("checked", true);
        }
        else {
            allProduct.prop("checked", false);
        }
    });


    
    $(document).on("keyup", ".quantity", function () {

        var txtQuantity = $(this);
        var qty = txtQuantity.val() ? parseInt(txtQuantity.val()) : 0;
        
        var txtLftQuantity = txtQuantity.parent().parent().find("#leftQnt");
        var leftQty = txtLftQuantity.text() ? parseInt(txtLftQuantity.text()) : 0;

        if (qty > leftQty) {
            alert("Only " + leftQty + " quantity remaining to shipped");           
            return;
        }

        var txtMetric = txtQuantity.parent().parent().find("#metric").text();

        var txtWeight = txtQuantity.parent().parent().find("#weight");
        var weight = txtWeight.val() ? parseInt(txtWeight.val()) : 0;

        var txtTotalQty = txtQuantity.parent().parent().find("#totalQty");
        var totalQuantity = txtTotalQty.val() ? parseInt(txtTotalQty.val()) : 0;

        var txtPrice = txtQuantity.parent().parent().find("#price");
        var Price = txtPrice.val() ? parseFloat(txtPrice.val()) : 0;

        var txtTotal = txtQuantity.parent().parent().find("#total");
        var finalPrice = txtTotal.val() ? parseFloat(txtTotal.val()) : 0;

        if (txtMetric == "gm" || txtMetric == "ml")
        {
            weight = weight / 1000;
        }
        var TotalQnty = weight * qty;
        txtTotalQty.val(TotalQnty);
        
        
        if (Price > 0 && finalPrice > 0 || Price > 0)
        {            
            var total = Price * TotalQnty;
            txtTotal.val(total);
            calculateGrandTotal();
        }
        else {

        }
    });



    function calculateFinalPrice(obj) {

        var el = $(obj);
        var txtPrice = el.val() ? parseFloat(el.val()) : 0;

        var txtTotalQty = el.parent().parent().find("#totalQty");
        var totalQuantity = txtTotalQty.val() ? parseFloat(txtTotalQty.val()) : 0;
        var total = txtPrice * totalQuantity;

        var txtTotal = el.parent().parent().find("#total");

        txtTotal.val(total);
        calculateGrandTotal();
    }


    //grand total calculate function
    function calculateGrandTotal() {
        var txtGrandTotal = $("#GrandTotal");
        var gt = 0;
        $(".total").each(function () {
            var t = $(this).val() ? parseInt($(this).val()) : 0;
            gt += t;
        });
        txtGrandTotal.val(gt.toFixed(0));
    }
