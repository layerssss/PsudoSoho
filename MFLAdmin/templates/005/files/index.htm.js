for(var i in root.lodgePhotoTypes){
	var type=root.lodgePhotoTypes[i];
	type.firstImgSmall=type.lodgeTypeAlbum.length?type.lodgeTypeAlbum[0].lodgePhotoUrlSmall:root.BaseUrl+'assets/shared/img/noimg.png';
}
for(var i in root.lodgeRooms){
    root.lodgeRooms[i].roomAttributesString='';
        for(var j in root.lodgeRooms[i].roomAttributes){
            var attr=root.lodgeRooms[i].roomAttributes[j]
            root.lodgeRooms[i].roomAttributesString+=
            '<li><span class="icon"><img src="'+root.baseUrl+'assets/shared/img/blank.gif" style="width:32px;height:32px;background-image:url('+root.adminBaseUrl+'/Style/lodge/icons_sprite.png);'+attr.attributeIcon+'" /></span><span class="attributeName">'+attr.attributeName+':</span><span class="optionName">'+attr.optionName+'</span></li>';
        }
}