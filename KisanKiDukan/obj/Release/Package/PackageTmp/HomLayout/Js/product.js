/// <reference path="jquery-2.1.4.min.js" />


$("#ProductName").keyup(function () {
    var pageVal = $(this).val();
    // var arr = pageVal.split(' ');
    pageVal = pageVal.replace(/ /g, "-");
    $("#url").val(pageVal.toLowerCase());
});

$('#checkAttr').click(function (e) {
    var mainStock = $("#mainStock");
    var attrDiv = $("#AttrDiv");
    if ($(this).prop("checked") == true)
    {
        mainStock.hide();
        attrDiv.show();
    }
    else
    {
        mainStock.show();
        attrDiv.hide();
    }
});

// Add new attribute section
$("#addNewabt").click(function () {
    debugger
    let table = $("#attrTable");
    let lastRow = table.find(".attrRow").last();
    let firstInput = lastRow.find("input").first();
    let nameOfFirstInput = firstInput.attr("name");
    let currentIndex = parseInt(nameOfFirstInput.replace(/[^\d.]/g, ''));
    let nextIndex = currentIndex + 1;
    let nextRow = lastRow.clone();
    nextRow.find("input").each(function () {
        var currentInput = $(this);
        currentInput.val('');
        var name = currentInput.attr("name");
        var newName=name.replace(currentIndex, nextIndex);
        currentInput.attr("name", newName);
    });
    nextRow.find("td").last().html('<span class="btn btn-danger" id="btnDeleteAttr"><i class="fa fa-trash-o"></i></span>');
    nextRow.find("#valueHolder").html('');
    table.append(nextRow);
});

// deleting attribue
$("#attrTable").on("click", "#btnDeleteAttr", function () {
    debugger
    var btn = $(this);
    var currentRow = btn.parent().parent();
    // finding all rows after this row ad increase their index by 1
    currentRow.nextAll().find("input").each(function () {
        var row = $(this);
        var name = row.attr('name');
        var index = parseInt(name.replace(/[^\d.]/g, ''));
        var nextIndex = index - 1;
        var newName = name.replace(index, nextIndex);
        row.attr('name', newName);
        console.log(row.attr("name"));
    });
    currentRow.remove();
});

// add new Value for an attribute


$("#attrTable").on("click", "#btnAddValue", function () {
    debugger
    var btn = $(this);
    var currentRow = btn.parent().parent();
    // first input of current row
    var firstInput = currentRow.find("input").first();
    var firstInputName = firstInput.attr("name");
    // find index of first input
    var firstInputIndex = parseInt(firstInputName.replace(/[^\d.]/g, ''));

    //find currentValueIndex ,setting by default 0 for first time click of this button
    var currentValueIndex = 0;
    var valueHolder = currentRow.find("#valueHolder");
    
    var valueHolderText = $.trim(valueHolder.html());
    if (valueHolderText.length < 1)
    {
        let tblWrapper = `<table class="table" id='tblWrapper'></table>`;
        valueHolder.append(tblWrapper);    
    }

    else{
        // find first input of table inside value holder
        let tbl = valueHolder.find('#tblWrapper');
        // find  last row
        let lastRow = tbl.find('tr').last();
        // find index of that row
        var index = parseInt(lastRow.attr('index'));
        // set currentValueIndex = index+1
        currentValueIndex = index + 1;
    }

    let tbl = valueHolder.find('#tblWrapper');
    // appent value row to table
    tbl.append(getValueRow(firstInputIndex, currentValueIndex))

});


// function for getting row for add attribute Value
function getValueRow(firstInputIndex, currentValueIndex) {
    var row = `<tr>
        <th>Value *</th>
        <th>Images</th>
        </tr>
          <tr index='${currentValueIndex}'>
            <td>
                 <input class ="form-control" name="PAttr[${firstInputIndex}].PAtValue[${currentValueIndex}].Value" placeholder="Red/S" type="text"  value="">
            </td>
            <td>
                <input class ="form-control" multiple="multiple" name="PAttr[${firstInputIndex}].PAtValue[${currentValueIndex}].PAtrImage" type="file" value="">
            </td>
        </tr>`;
    return row;
}



$("#attrTable").on("click", "#btnDeleteAttrValue", function () {

    var btn = $(this);
    var currentRow = btn.parent().parent();
    // finding all rows after this row ad increase their index by 1
    currentRow.nextAll().find("input").each(function () {
        var input = $(this);
        let name = input.attr('name');
        let nameArr = name.split('.');

        // getting index value of current Value
        let arr1 = nameArr[1];
        let index = parseInt(arr1.replace(/[^\d.]/g, ''));
        let newIndex = index - 1;
        let newArr1 = arr1.replace(index, newIndex);
        // replace 1 index value of arry with new one
        nameArr[1] = newArr1;

        // now create the new name for that input
        let newName = nameArr.join('.');

        // replace name of this input with new name
        input.attr("name", newName);
       
    });
    currentRow.remove();

   

});