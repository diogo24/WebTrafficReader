/*
Bug fix with JQuery date validations
http://stackoverflow.com/questions/12845817/asp-net-mvc-set-validation-date-format-fails-on-chrome
http://www.telerik.com/forums/datepicker-format-will-not-validate-even
http://forums.asp.net/t/1831712.aspx?Datetime+validation+headache+in+MVC+4
*/
$(document).ready(function () {
    try {
        jQuery.validator.addMethod(
            'date',
            function (value, element, params) {
                if (this.optional(element)) {
                    return true;
                };
                var result = false;
                try {
                    var date = kendo.parseDate(value, "dd/MM/yyyy");

                    result = true;
                    if (!date) {
                        result = false;
                    }
                } catch (err) {
                    result = false;
                }
                return result;
            },
            ''
        );
    }
    catch(err)
    { }
});
