(function () {
    $(document).ajaxError(function (event, jqxhr, settings, exception) {
        if (jqxhr.responseText) {
            try {
                var result = $.parseJSON(jqxhr.responseText);
                alert(result.message)
            } catch (ex) {

            }
        }
    });

    $.request = function (url, data, success, error) {
        $.ajax({
            url: url,
            data: data,
            dataType: "json",
            method: "POST",
            headers: { "_ajax_": true },
            success: function (data, textStatus, jqXHR) {
                success(data);
            }
        }).fail(function (xhr) {
            var data = eval("(" + xhr.responseText + ")");
            if (error) {
                error(data);
            }
        });
        //$.post(url, data, success, "json").fail(error);
    };
})();