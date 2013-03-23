var func = function (option) {
    var defaultOption = {
        "a": 1,
        "c": 3,
        "d": 5
    };
    option = $.extend(defaultOption, option);
    alert(option.b);
    alert(option.d);
};
func({
    a:1,
    b:2,
    c:3
});