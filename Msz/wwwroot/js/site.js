var datePickerOptions = {
    format: "dd.mm.yyyy",
    weekStart: 1,
    maxViewMode: 2,
    todayBtn: "linked",
    language: "ru",
    orientation: "bottom auto",
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
    $(".date input").datepicker(datePickerOptions).on('changeDate', function () {
        $(this).focusout();
    });
    $(".date input").inputmask("99.99.9999");

    $(".msz-snils-field").inputmask("999-999-999-99");

    $(".msz-amount-field").inputmask('Regex', { regex: "^[0-9]{1,6}(\\,\\d{1,2})?$" });

    $("#msz-category-file-btn").on("change", function () {
        $(this).closest("form").submit();
    });

    $(".msz-pagination li.disabled a").on("click", function (e) {
        e.preventDefault();
    });

    $(".msz-tool-panel .btn.disabled, .msz-tool-panel .ms-disabled").on("click", function (e) {
        e.preventDefault();
    });

    var reasonPersonTemplate = null;

    $(".msz-add-reason-person-btn").on("click", function (e) {
        if (reasonPersonTemplate === null) {
            $.get("/Receiver/GetEmptyReasonPerson", function (data) {
                reasonPersonTemplate = data;
                addReasonPerson(reasonPersonTemplate);
            });
        } else {
            addReasonPerson(reasonPersonTemplate);
        }
        e.preventDefault();
    });

    function showReasonPersonModal()
    {
        $(this).data("state", "shown");
        if ($(this).data("valid") === undefined) {
            $(this).data("valid", "invalid");
        }
        $(this).find(".date input").each(function (idx, elem) {
            if ($(elem).val() === "01.01.0001") $(elem).val('');
        });

        $(this).find("input, select").removeClass('input-validation-error');
        $(this).find('span.field-validation-error').text('');

        $(this).data("surname", $(this).find("input[id$='__Surname']").val());
        $(this).data("name", $(this).find("input[id$='__Name']").val());
        $(this).data("patronymic", $(this).find("input[id$='__Patronymic']").val());
        $(this).data("gender_id", $(this).find("select[id$='__GenderId']").val());
        $(this).data("birth_date", $(this).find("input[id$='__BirthDate']").val());
        $(this).data("snils", $(this).find("input[id$='__Snils']").val());
        $(this).data("kinship_id", $(this).find("select[id$='__KinshipRelationId']").val());
    }

    function hideReasonPersonModal() {
        $(this).data("state", "hidden");
        if ($(this).data("valid") === "invalid") {
            $(this).remove();
        } else {
            $(this).find("input[id$='__Surname']").val($(this).data("surname"));
            $(this).find("input[id$='__Name']").val($(this).data("name"));
            $(this).find("input[id$='__Patronymic']").val($(this).data("patronymic"));
            $(this).find("select[id$='__GenderId']").val($(this).data("gender_id"));
            $(this).find("input[id$='__BirthDate']").val($(this).data("birth_date"));
            $(this).find("input[id$='__Snils']").val($(this).data("snils"));
            $(this).find("select[id$='__KinshipRelationId']").val($(this).data("kinship_id"));
            var tbody = $(".msz-reason-persons-table-wrapper table tbody");
            if (tbody.find("tr td.msz-reason-persons-table-empty").length > 0) {
                tbody.empty();
            }
            var index = $(this).index();
            var gender_id = $(this).data("gender_id");
            var gender = $("#Receiver_GenderId option[value='"+gender_id+"']").text();
            var tr = "<tr>" +
                "<td>" + $(this).data("surname") + " " + $(this).data("name") +
                ($(this).data("patronymic") !== "" ? " " + $(this).data("patronymic") : "") + "</td>" +
                "<td>" + $(this).data("birth_date") + "</td>" +
                "<td>" + $(this).data("snils") + "</td>" +
                "<td>" + gender + "</td>" +
                "<td><div class='btn-group' role='group'>" +
                "<button class='btn btn-success msz-reason-person-edit-btn'><span class='glyphicon glyphicon-pencil' aria-hidden='true'></span></button>" +
                "<button class='btn btn-danger msz-reason-person-delete-btn'><span class='glyphicon glyphicon-remove' aria-hidden='true'></span></button>" +
                "</div></td>" +
                "</tr>";

            if ((tbody.find("tr").length - 1) < index) {
                tbody.append(tr);
            } else {
                $(tbody.find('tr')[index]).replaceWith(tr);
            }

        }
    }

    function addReasonPerson(reasonPersonTemplate) {
        $(".msz-reason-persons-modal-wrapper").append(reasonPersonTemplate);
        updateReasonPersonIndexes($(".msz-reason-persons-modal-wrapper"));
        updateValidation();
        var modal = $(".msz-reason-persons-modal-wrapper .modal").last();
        modal.find('.date input').datepicker(datePickerOptions);
        modal.find(".msz-snils-field").inputmask("999-999-999-99");
        modal.find(".date input").inputmask("99.99.9999");
        modal.on('show.bs.modal', showReasonPersonModal);
        modal.on('hidden.bs.modal', hideReasonPersonModal);
        modal.keypress(function (e) {
            if (e.keyCode === 13) {
                modal.find(".msz-save-btn").click();
                e.preventDefault();
            }
        });
        modal.modal('show');
    }

    $(".modal").on('show.bs.modal', showReasonPersonModal);
    $(".modal").on('hidden.bs.modal', hideReasonPersonModal);
    $(".modal").keypress(function (e) {
        if (e.keyCode === 13) {
            modal.find(".msz-save-btn").click();
            e.preventDefault();
        }
    });
    $(".modal").data('valid', 'valid');

    $('body').on('click', '.msz-save-btn', function (e) {
        var modal = $(this).closest(".modal");
        $("form").valid();
        var validator = $("form").validate();

        var snils = $(modal).find(".msz-snils-field").val();
        var isValid = snilsIsValid(snils);
        errors = {};
        if (!isValid) {
            errors[$(modal).find(".msz-snils-field").prop("name")] = "Указано некорректно значение";
            validator.showErrors(errors);
            e.preventDefault();
        }
        if ($("form").find(".msz-snils-field").filter(function (idx, elem) { return $(elem).val() === snils; }).length > 1) {
            errors[$(modal).find(".msz-snils-field").prop("name")] = "Лицо с таким СНИЛС уже добавлено";
            validator.showErrors(errors);
            e.preventDefault();
        }
        $("form").find("input, select").each(function (idx, elem) {
            if ($(elem).closest('.modal').length === 0) {
                $(elem).removeClass('input-validation-error');
            }
        });
        /*
        $('span.field-validation-error').each(function (idx, elem) {
            var input = $(elem).closest('div').find('input');
            if (!input.hasClass('input-validation-error')) {
                $(elem).text('/');
            }
        });*/

        var invalidInputs = modal.find(".input-validation-error");
        if (invalidInputs.length === 0) {
            modal.data("valid", "valid");
            modal.data("surname", modal.find("input[id$='__Surname']").val());
            modal.data("name", modal.find("input[id$='__Name']").val());
            modal.data("patronymic", modal.find("input[id$='__Patronymic']").val());
            modal.data("gender_id", modal.find("select[id$='__GenderId']").val());
            modal.data("birth_date", modal.find("input[id$='__BirthDate']").val());
            modal.data("snils", modal.find("input[id$='__Snils']").val());
            modal.data("kinship_id", modal.find("select[id$='__KinshipRelationId']").val());
            modal.modal('hide');
        }
    });


    $("body").on('click', '.msz-reason-person-edit-btn', function (e) {
        var index = $(this).closest('tr').index();
        var modal = $(".msz-reason-persons-modal-wrapper .modal")[index];
        $(modal).modal('show');
        e.preventDefault();
    });

    $("body").on('click', '.msz-reason-person-delete-btn', function (e) {
        var tbody = $(this).closest('tbody');
        var index = $(this).closest('tr').index();
        var modal = $(".msz-reason-persons-modal-wrapper .modal")[index];
        $(modal).remove();
        $(this).closest('tr').remove();
        updateReasonPersonIndexes($(".msz-reason-persons-modal-wrapper"));
        if (tbody.find('tr').length === 0) {
            tbody.append("<tr><td colspan='5' class='msz-reason-persons-table-empty'>Сведения отсутствуют</td></tr>");
        }
        e.preventDefault();
    });

    function updateValidation() {
        var form = $("form")
            .removeData("validator")
            .removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse(form);
        form.validate();
    }

    function updateReasonPersonIndexes(wrapper) {
        var namePropRegex = /(Receiver.ReasonPersons)\[\d+\]/;
        var idPropRegex = /(Receiver.ReasonPersons)_\d+__/;
        var modals = $(wrapper).find(".modal");
        modals.each(function (modalIdx, modalElem) {
            updateModal(modalIdx, modalElem, namePropRegex, idPropRegex);
        });
    }

    function updateModal(idx, modal, namePropRegex, idPropRegex) {
        $(modal)
            .find("[name]")
            .filter(function (fieldIdx, field) {
                return $(field).prop("name").match(namePropRegex) !== null;
            })
            .each(function (fieldIdx, field) {
                var name = $(field).prop("name").replace(namePropRegex, "$1[" + idx + "]");
                $(field).prop("name", name);
                var id = $(field).prop("id").replace(idPropRegex, "$1_" + idx + "__");
                $(field).prop("id", id);
                var wrapper = $(field).closest("div");
                var label = wrapper.prev("label");
                if (label.length === 0) {
                    label = wrapper.parent().prev("label");
                }
                var spanErro = wrapper.find("span[data-valmsg-for]");
                if (spanErro.length === 0) {
                    spanErro = wrapper.parent().find("span[data-valmsg-for]");
                }
                label.prop("for", id);
                spanErro.attr("data-valmsg-for", name);
            });
        $(modal).prop("id", "mszReasonPersonsModal" + idx);
        $(modal).attr("aria-labelledby", "mszReasonPersonsModal" + idx + "Title");
        $(modal).find(".modal-header h4").prop("id", "mszReasonPersonsModal" + idx + "Title");
    }

    $("body").on("click",
            ".msz-show-date-picker-btn",
            function () {
                $(this).closest(".date.input-group").find("input").datepicker("show");
        });

    $("#Receiver_AssigmentFormId").on('change', function () {
        var equivalentAmount = $("#Receiver_EquivalentAmount").closest(".form-group");
        if ($(this).val() === "3") {
            equivalentAmount.show();
        } else {
            equivalentAmount.hide();
        }
    });

    $("#Receiver_AssigmentFormId").change();

    var categoryOptions = $("#Receiver_CategoryId option[value], #FilterOptions_CategoryId option[value]").clone(true);
    $("#Receiver_CategoryId option[value], #FilterOptions_CategoryId option[value]").remove();

    $("#Receiver_MszId, #FilterOptions_MszId").on('change', function () {
        var mszId = parseInt($(this).val());
        var mszOption = $(this).find("option[value='" + mszId + "']");
        var title = "";
        if (mszOption.length > 0) {
            title = mszOption.text();
        }
        $(this).prop("title", title);
        var filteredCategoryOptions = $(categoryOptions).filter(function (idx, elem) {
            return $(elem).data('msz-id') === mszId;
        });
        $("#Receiver_CategoryId option[value], #FilterOptions_CategoryId option[value]").remove();
        $("#Receiver_CategoryId, #FilterOptions_CategoryId").append(filteredCategoryOptions);
    });

    $("#Receiver_MszId, #FilterOptions_MszId").change();

    $("body").on("click", ".msz-receiver-delete-btn", function (e) {
        var id = $(this).data('id');
        var snp = $(this).closest("tr").find(".msz-receiver-snp").text();
        var modal = $("#deleteReceiverModal");
        modal.find("#deleteReceiverSnp").text(snp);
        modal.find("form input[name='id']").val(id);
        modal.modal('show');
        e.preventDefault();
    });

    $(".msz-search-btn, .msz-dd-search-btn ").on("click", function (e) {
        e.preventDefault();
        var modal = $("#mszFilterModal");
        modal.modal('show');
    });

    $(".msz-submit-btn").on("click", function (e) {
        var form = $(this).closest("form");
        form.find("input, select").each(function (idx, elem) {
            $(elem).removeClass('input-validation-error');
        });
        $('span.field-validation-error').each(function (idx, elem) {
            var input = $(elem).closest('div').find('input');
            if (!input.hasClass('input-validation-error')) {
                $(elem).text('');
            }
        });

        var validator = form.validate();
        var formValid = form.valid();
        var startDate = convertStrToDate($("#Receiver_StartDate").val());
        var endDate = convertStrToDate($("#Receiver_EndDate").val());
        var errors = {};
        if (startDate !== null && endDate !== null) {
            if (endDate < startDate) {
                errors["Receiver.EndDate"] = "Дата окончания не может быть меньше даты начала";
                formValid = false;
            }
        }

        var snils = $("#Receiver_Snils").val();
        var isValid = snilsIsValid(snils);
        if (!isValid) {
            errors["Receiver.Snils"] = "Указано некорректно значение";
            formValid = false;
        }

        var assigmentFormId = $("#Receiver_AssigmentFormId").val();
        var amount = $("#Receiver_Amount").val();
        if (assigmentFormId === "3" && (amount.indexOf(',') !== -1 || parseInt(amount) > 100)) {
            errors["Receiver.Amount"] = "Указанное значение не соответствует форме предоставления";
            formValid = false;
        }


        if (!formValid) {
            e.preventDefault();
            validator.showErrors(errors);
        }
        return formValid;
    });

    $("#Receiver_EndDate, #Receiver_StartDate").on("focusout", function (e) {
        var form = $(this).closest("form");

        var validator = form.validate();

        $("#Receiver_EndDate").removeClass('input-validation-error');
        $("#Receiver_EndDate").closest("div.input-group").next().text('');

        var startDate = convertStrToDate($("#Receiver_StartDate").val());
        var endDate = convertStrToDate($("#Receiver_EndDate").val());

        if (startDate === null || endDate === null) return;

        var errors = {};
        if (endDate < startDate) {
            errors["Receiver.EndDate"] = "Дата окончания не может быть меньше даты начала";
            validator.showErrors(errors);
            $("#Receiver_StartDate").removeClass('input-validation-error');
            $("#Receiver_StartDate").closest("div.input-group").next().text('');
            e.preventDefault();
            return false;
        }
    });

    $("body").on("focusout", ".msz-snils-field", function (e) {
        var form = $(this).closest("form");

        var validator = form.validate();

        $(this).removeClass('input-validation-error');
        $(this).next().text('');

        var snils = $(this).val();

        var isValid = snilsIsValid(snils);

        var errors = {};
        if (!isValid) {
            errors[$(this).prop("name")] = "Указано некорректно значение";
            validator.showErrors(errors);
            e.preventDefault();
            return false;
        }
    });

    $("body").on("focusout", "#Receiver_AssigmentFormId, #Receiver_Amount", function (e) {

        var form = $(this).closest("form");

        var validator = form.validate();

        $("#Receiver_Amount").removeClass('input-validation-error');
        $("#Receiver_Amount").next().text('');

        var assigmentFormId = $("#Receiver_AssigmentFormId").val();
        var amount = $("#Receiver_Amount").val();
        var errors = {};
        if (assigmentFormId === "3" && (amount.indexOf(',') !== -1 || parseInt(amount) > 100)) {
            errors["Receiver.Amount"] = "Указанное значение не соответствует форме предоставления";
            validator.showErrors(errors);
            e.preventDefault();
            return false;
        }
    });

    function convertStrToDate(strDate)
    {
        if (strDate === null) return null;
        var dateParts = strDate.split(".");
        if (dateParts.length !== 3) {
            return null;
        }
        return new Date(dateParts[2] | 0, (dateParts[1] | 0) - 1, dateParts[0] | 0);
    }

    function snilsIsValid(snils) {
        if (typeof snils === 'number') {
            snils = snils.toString();
        } else if (typeof snils !== 'string') {
            snils = '';
        }
        snils = snils.replace(/-/g, '');
        if (snils.length !== 11 || /[^0-9]/.test(snils)) {
            return false;
        } 

        var sum = 0;
        for (var i = 0; i < 9; i++) {
            sum += parseInt(snils[i]) * (9 - i);
        }
        var checkDigit = 0;
        if (sum < 100) {
            checkDigit = sum;
        } else
        if (sum === 100)
        {
            checkDigit = 0;
        } else {
            checkDigit = parseInt(sum % 101);
            if (checkDigit === 100) {
                checkDigit = 0;
            }
        }
        if (checkDigit === parseInt(snils.slice(-2))) {
            return true;
        } else {
            return false;
        }
    }

});