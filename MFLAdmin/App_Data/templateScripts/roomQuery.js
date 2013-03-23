
root.roomAttributesStringForRoomQuery='';
for (var j in root.roomAttributes) {
    var attr = root.roomAttributes[j]
    root.roomAttributesStringForRoomQuery +=
        '<td class="rq-roomAttribute"><img src="' + root.baseUrl + 'assets/shared/img/blank.gif" title="' + attr.optionName + '" style="width:32px;height:32px;background-image:url(' + root.adminBaseUrl + '/Style/lodge/icons_sprite.png);' + attr.attributeIcon + '" /></td>';
}