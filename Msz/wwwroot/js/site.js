var datePickerOptions = {
    format: "dd.mm.yyyy",
    weekStart: 1,
    maxViewMode: 2,
    todayBtn: "linked",
    language: "ru",
    orientation: "bottom auto",
    daysOfWeekDisabled: "0,6",
    autoclose: true,
    todayHighlight: true,
    startDate: "01/01/1753"
};

if ($.validator !== undefined) {
    $.extend($.validator.methods, {
        date: function (value, element) {
            if (this.optional(element) && value === "") {
                return true;
            }
            var dateParts = value.split(".");
            if (dateParts.length !== 3) {
                return false;
            }
            return !isNaN(Date.parse(dateParts[2] + "/" + dateParts[1] + "/" + dateParts[0]));
        },

        number: function (value, element) {
            return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
        }
    });
}

$(document).ready(function () {
    $(".date input").datepicker(datePickerOptions);

    $("#msz-category-file-btn").on("change", function () {
        $(this).closest("form").submit();
    });

    $(".msz-pagination li.disabled a").on("click", function (e) {
        e.preventDefault();
    });

    $(".msz-tool-panel .btn.disabled, .msz-tool-panel .ms-disabled").on("click", function (e) {
        e.preventDefault();
    });

    $(".msz-snils-field").inputmask("999-999-999-99");

    $(".msz-amount-field").inputmask('Regex', { regex: "^[0-9]{1,6}(\\,\\d{1,2})?$" });


    $("body")
        .on("click",
            ".msz-show-date-picker-btn",
            function () {
                $(this).closest(".date.input-group").find("input").datepicker("show");
            });
});