$.validator.unobtrusive.adapters.addSingleVal("required", "string");
$.validator.addMethod("required", function (value, element, required) {
    return true;
});  