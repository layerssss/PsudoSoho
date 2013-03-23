root.PageTitle=root.roomName+'-'+root.lodgeName+'-'+root.TemplatePropertyTitleSuffix;
for(var i in root.lodgeRooms){
    root.lodgeRooms[i].roomAttributes='';
    if(root.lodgeRooms[i].roomId==root.roomId){
        for(var j in root.roomAttributes){
            var attr=root.roomAttributes[j]
            root.lodgeRooms[i].roomAttributes+=
            '<li><span class="icon"><img src="'+root.baseUrl+'assets/shared/img/blank.gif" style="width:32px;height:32px;background-image:url('+root.adminBaseUrl+'/Style/lodge/icons_sprite.png);'+attr.attributeIcon+'" /></span><span class="attributeName">'+attr.attributeName+':</span><span class="optionName">'+attr.optionName+'</span></li>';
        }
    }
}