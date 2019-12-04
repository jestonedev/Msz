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

        $(this).data("surname", $(this).find("input[id$='__Surname']").val());
        $(this).data("name", $(this).find("input[id$='__Name']").val());
        $(this).data("patronymic", $(this).find("input[id$='__Patronymic']").val());
        $(this).data("gender_id", $(this).find("select[id$='__GenderId']").val());
        $(this).data("birth_date", $(this).find("input[id$='__BirthDate']").val());
        $(this).data("snils", $(this).find("input[id$='__Snils']").val());
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

    $('body').on('click', '.msz-save-btn', function () {
        var modal = $(this).closest(".modal");
        $("form").valid();
        $("form").find("input, select").each(function (idx, elem) {
            if ($(elem).closest('.modal').length === 0) {
                $(elem).removeClass('input-validation-error');
            }
        });
        $('span.field-validation-error').each(function (idx, elem) {
            var input = $(elem).closest('div').find('input');
            if (!input.hasClass('input-validation-error')) {
                $(elem).text('');
            }
        });

        var invalidInputs = modal.find(".input-validation-error");
        if (invalidInputs.length === 0) {
            modal.data("valid", "valid");
            modal.data("surname", modal.find("input[id$='__Surname']").val());
            modal.data("name", modal.find("input[id$='__Name']").val());
            modal.data("patronymic", modal.find("input[id$='__Patronymic']").val());
            modal.data("gender_id", modal.find("select[id$='__GenderId']").val());
            modal.data("birth_date", modal.find("input[id$='__BirthDate']").val());
            modal.data("snils", modal.find("input[id$='__Snils']").val());
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


});