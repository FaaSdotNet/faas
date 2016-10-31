var optionsCount = 0;

$(document).ready(function () {

    $("#Type").change(function () {
        if ($("#Type").val() != 0 && $("#Type").val() != 2)
        {
            $("#add").hide();
            $("#delete").hide();
            for (var i = 1; i <= optionsCount; i++)
            {
                $("#option" + i).hide();
            }
            $("#Options").val("");
            $("#na").show();
        }
        else
        {
            $("#add").show();
            for (var i = 1; i <= optionsCount; i++)
            {
                $("#option" + i).show();
            }
            if (optionsCount > 0)
            {
                $("#delete").show();
            }
            $("#na").hide();
        }
    })

    $("#add").click(function () {
        $("#delete").show();
        optionsCount++;
        var optionHtml = '<div id="option' + optionsCount + '"><span>Option ' + optionsCount + ": </span>";
        optionHtml += '<input id="input' + optionsCount + '" type="text" disabled>&nbsp;&nbsp;';
        optionHtml += '<a id="edit' + optionsCount + '" href="#"><i class="glyphicon glyphicon-pencil"></i></a></div>';

        if (optionsCount == 1) {
            $("#Options").after(optionHtml);
        }
        else {
            $("#option" + (optionsCount - 1)).after(optionHtml);
        }

        $("#edit" + optionsCount).click(function () {
            var id = $(this).attr("id");
            id = id.substr(4);
            if ($("#input" + id).attr("disabled"))
            {
                $("#input" + id).attr('disabled', false);
                $(this).children().first().toggleClass("glyphicon glyphicon-ok");
                $(this).children().first().toggleClass("glyphicon glyphicon-pencil");
            }
            else
            {
                $("#input" + id).attr('disabled', true);
                $(this).children().first().toggleClass("glyphicon glyphicon-ok");
                $(this).children().first().toggleClass("glyphicon glyphicon-pencil");
            }
        });
    });

    $("#delete").click(function () {
        $("#option" + optionsCount).remove();
        optionsCount--;

        if (optionsCount == 0)
        {
            $("#delete").hide();
        }
    });

    $('#coolForm').submit(function () {
        
        var optionsDictionary = {};
        for (var i = 1; i <= optionsCount; i++)
        {
            optionsDictionary[i] = $("#input" + i).val();
        }
        var optionsJSON = JSON.stringify(optionsDictionary);
        $("#Options").val(optionsJSON);

        return true;
    });

    $("#na").hide();
    $("#delete").hide();
});