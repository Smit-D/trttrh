// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('#CountryId').change(function (event) {
    if (event.target.value > 0) {
        $.ajax({
            url: `/Home/GetStateListByCountryIdJson/?countryId=${event.target.value}`,
            type: 'get',
            success: function (result) {
                let html = `<option value="">Select State</option>`;
                debugger
                if (result != null) {
                    $.each(result, function (key, value) {
                        html += `<option value="${value.value}">${value.text}</option>`;
                    });
                    $('#StateId').html(html);
                }
            },
            error: function () {
                console.log(err);
            },
        });

    }
});
$('#StateId').change(function (event) {
    if (event.target.value > 0) {
        $.ajax({
            url: `/Home/GetCityListByStateIdJson/?stateId=${event.target.value}`,
            type: 'get',
            success: function (result) {
                if (result != null) {
                    let html = `<option value="">Select City</option>`;


                    $.each(result, function (index, value) {
                        html += `<option value="${value.value}">${value.text}</option>`;
                    });
                    $('#CityId').html(html);
                }
            },
            error: function () {
                console.log(err);
            },
        });

    }
});