var mapLoaded = false;
$(function () {
    $(".mfl-refresh-btn").click(function () {
        MFLAjax({
            action: "更新前台页面",
            success: function (json) {
                refreshFunc();
            }
        });
        return false;
    });
    var refreshFunc = function () {
        MFLAjax({
            action: "获取旅馆信息",
            success: function (json) {
                if (json.refreshing) {
                    setTimeout(refreshFunc, 300);
                } else {
                    var title = (json.lastRefreshedTime.indexOf('更新成功') == -1 ? '应用更改失败' : '应用更改成功');
                    MFLDialog(title, '应用更改结果：' + json.lastRefreshedTime);
                }
            }
        });
    };
    MFLAjax.globalJsonFilter = function (json) {
        if (json.lodgeChanged) {
            $('.mfl-refresh-btn').removeClass('ui-state-disabled');
        } else {
            $('.mfl-refresh-btn').addClass('ui-state-disabled');
        }
        return json;
    };
    $(".mfl-lodge-root").data("MFLPageLoader", function (query, callback) {
        $(".preview table tr:odd").css({ "backgroundColor": "#f5f5f5" });
        var refreshFunc = function (callback) {
            MFLAjax({
                action: "获取旅馆信息",
                success: function (json) {
                    $(".mfl-lodge-root.mfl-currentPage .mfl-refresh-status").text(json.refreshStatus);
                    $(".mfl-lodge-root.mfl-currentPage .mfl-refresh-lastRefreshedTime").text(json.lastRefreshedTime);
                    var a = $(".mfl-lodge-root.mfl-currentPage .mfl-refresh-btn");
                    if (json.refreshing) {
                        a.hide();
                    } else {
                        a.show();
                    }
                    for (var key in json) {
                        if (typeof (json[key]) == 'string' || typeof (json[key]) == 'number') {
                            $('.mfl-lodge-root.mfl-currentPage .info p.mfl-' + key)
                            .removeClass('loading').text(String(json[key]));
                        }
                    }
                    if (json.numRooms == 0) {
                        MFLWizardLoad();
                    }
                    callback();
                }
            });
        }
        $('.mfl-lodge-root.mfl-currentPage .info>ul>li')[MFLTooltipPlugin]();
        $(".mfl-lodge-root.mfl-currentPage .mfl-refresh-refresh").click(function () {
            refreshFunc(function () { });
            return false;
        });
        refreshFunc(function () {
            callback($(".mfl-lodge-root"));
        });
    });
    $(".mfl-lodge-refresh").data("MFLPageLoader", function (query, callback) {
        $(".mfl-lodge-refresh.mfl-currentPage .refreshStatic").click(function () {
            MFLAjax({
                action: "更新前台页面",
                success: function () {
                }
            });
        });
        callback($(".mfl-lodge-refresh"));
    });
    $(".mfl-lodge-attributes").data("MFLPageLoader", function (query, callback) {
        MFLList({
            action: "获取特性",
            containerJ: $(".mfl-lodge-attributes.mfl-currentPage .mfl-lodge-attributes-list"),
            callback: function (json) {
                $(".mfl-lodge-attributes.mfl-currentPage .mfl-list-item .mfl-data-attributeId").click(function () {
                    var root = this;
                    var data = $(this).data("MFLData-attributeId");
                    MFLDialog("确认删除", "即将删除该特性", null, function () {
                        MFLAjax({
                            action: "删除特性",
                            query: "&id=" + data,
                            success: function () {
                                MFLHash();
                            }
                        });
                    });
                });
                $('.mfl-lodge-attributes.mfl-currentPage .mfl-list-item>.edit a')[MFLTooltipPlugin]();
                callback($(".mfl-lodge-attributes"));
            }
        });
    });
    $(".mfl-lodge-lodgeAlbum").data("MFLPageLoader", function (query, callback) {
        MFLList({
            action: "旅馆相册获取照片",
            containerJ: $(".mfl-lodge-lodgeAlbum.mfl-currentPage .mfl-lodgeAlbumList"),
            callback: function (json) {
                MFLAjax({
                    "action": "获取相册状态",
                    "success": function (j) {
                        $(".mfl-lodge-lodgeAlbum.mfl-currentPage .avaPhoto").text(j.albumStatus.avaPhoto);
                        $(".mfl-lodge-lodgeAlbum.mfl-currentPage .capPhoto").text(j.albumStatus.capPhoto);
                        $('.mfl-lodge-lodgeAlbum.mfl-currentPage .progressbar').progressbar({
                            value: j.albumStatus.percentage
                        });
                        $(".mfl-lodge-lodgeAlbum.mfl-currentPage .mfl-list-item .mfl-data-lodgePhotoId").click(function () {
                            var root = this;
                            var data = $(this).data("MFLData-lodgePhotoId");
                            MFLDialog("确认删除", "即将删除该照片", null, function () {
                                MFLAjax({
                                    action: "旅馆相册删除照片",
                                    query: "&id=" + data,
                                    success: function () {
                                        MFLHash();
                                    }
                                });
                            });
                        })[MFLTooltipPlugin]();
                        MFLList({
                            containerJ: $(".mfl-lodge-lodgeAlbum.mfl-currentPage .mfl-photoTypeList"),
                            action: '旅馆相册获取分类',
                            callback: function (json) {
                                $(".mfl-lodge-lodgeAlbum.mfl-currentPage .mfl-photoTypeList>.mfl-list-item .mfl-not-progressbar-percentage").each(function (i, e) {
                                    if (json.listItems[i].percentage != -1) {
                                        $(this).progressbar({
                                            value: json.listItems[i].percentage
                                        }).attr('title', '该分类为当前模版要求的特殊图片分类<br/>这个进度条是否达到100%表示该分类的图片数目是否已上传足够。')[MFLTooltipPlugin]();
                                    }
                                });
                                callback($(".mfl-lodge-lodgeAlbum"));
                            }
                        });
                    }
                });
            }
        });
    });

    $(".mfl-lodge-rooms").data("MFLPageLoader", function (query, callback) {
        MFLList({
            action: "获取房间",
            containerJ: $(".mfl-lodge-rooms.mfl-currentPage .mfl-roomList"),
            callback: function (json) {
                $(".mfl-lodge-rooms.mfl-currentPage .mfl-list-item .mfl-data-roomId").click(function () {
                    var root = this;
                    var data = $(this).data("MFLData-roomId");
                    MFLDialog("确认删除", "即将删除该房间", null, function () {
                        MFLAjax({
                            action: "删除房间",
                            query: "&id=" + data,
                            success: function () {
                                MFLHash();
                            }
                        });
                    });
                });
                $('.mfl-lodge-rooms.mfl-currentPage .mfl-list-item>.edit a')[MFLTooltipPlugin]();
                $(".mfl-lodge-rooms.mfl-currentPage .mfl-list-item .mfl-list-roomAttributes .mfl-list-item>img.mfl-alt-optionName")[MFLTooltipPlugin]();
                callback($(".mfl-lodge-rooms"));
            }
        });
    });
    $(".mfl-lodge-room").data("MFLPageLoader", function (query, callback) {
        MFLList({
            action: "获取房间",
            containerJ: $(".mfl-lodge-room.mfl-currentPage .mfl-roomList"),
            callback: function (json) {
                $(".mfl-lodge-room .mfl-lodge-title").text("房间：" + json.listItems[0].roomName);
                $(".mfl-lodge-room.mfl-currentPage .mfl-list-item .mfl-val-roomIcon").lodgeIcon();
                $(".mfl-lodge-room.mfl-currentPage .mfl-list-item form").submit(function () {
                    var data = $(this).serialize();
                    MFLAjax({
                        action: "修改房间",
                        query: "&id=" + query + "&" + data,
                        success: function () {
                            MFLHash(null, -1);
                        }
                    });
                });
                MFLList({
                    action: "获取房间特性",
                    query: "&id=" + query,
                    containerJ: $(".mfl-lodge-room.mfl-currentPage .mfl-list-item .mfl-roomAttributesList"),
                    callback: function (attributesJson) {
                        $(".mfl-lodge-room.mfl-currentPage .mfl-roomAttributesList .mfl-list-item .mfl-val-optionId").each(function () {
                            $(this).lodgeIcon({
                                'action': '获取选项',
                                'query': '&id=' + $(this).data("MFLData-attributeId"),
                                'fieldIcon': 'optionIcon',
                                'fieldText': 'optionName',
                                'fieldValue': 'optionId',
                                'gettingIcon': function (root) {
                                    return $(root).data("MFLData-optionIcon");
                                },
                                'gettingText': function (root) {
                                    return $(root).data("MFLData-optionName");
                                },
                                'collapsable': false,
                                'cssClassImg': 'mfl-hidden',
                                'diaplaySelected': true,
                                'cssClassA': 'mfl-class-optionIcon optionIcon$optionIcon$',
                                'cssClassSpan': 'mfl-class-optionIcon',
                                'showText': true,
                                'selected': function (root) {
                                    MFLAjax({
                                        action: "修改房间特性",
                                        query: "&roomId=" + json.listItems[0].roomId + "&optionId=" + $(root).val() + "&attributeId=" + $(root).data("MFLData-attributeId"),
                                        success: function () {
                                            $(root).prev().children('span').attr('class',
                                            $(root).next().next().find('.mfl-lodge-icon-selected').attr('class') + ' mfl-optionName');
                                        }
                                    });
                                }
                            });
                        });
                        $(".mfl-lodge-room.mfl-currentPage .mfl-roomAttributesList .mfl-list-item>span>.mfl-optionName").each(function (i, e) {
                            $(e).addClass('optionIcon' + attributesJson.listItems[i].optionIcon);
                        });
                        MFLList({
                            action: "房间相册获取照片",
                            query: "&roomId=" + query,
                            containerJ: $(".mfl-lodge-room.mfl-currentPage .mfl-list-item .mfl-roomAlbumList"),
                            callback: function () {
                                $(".mfl-lodge-room.mfl-currentPage .mfl-list-item .mfl-roomAlbumList .mfl-list-item .photo a.mfl-data-roomPhotoId")[MFLTooltipPlugin]();
                                MFLAjax({
                                    "action": "获取相册状态",
                                    "success": function (j) {
                                        $(".mfl-lodge-room.mfl-currentPage .mfl-list-item .avaPhoto").text(j.albumStatus.avaPhoto);
                                        $(".mfl-lodge-room.mfl-currentPage .mfl-list-item .capPhoto").text(j.albumStatus.capPhoto);
                                        $(".mfl-lodge-room.mfl-currentPage .mfl-list-item .progressbar").progressbar({
                                            value: j.albumStatus.percentage
                                        });
                                        $(".mfl-lodge-room.mfl-currentPage .mfl-list-item .mfl-roomAlbumList .mfl-list-item .mfl-data-roomPhotoId").click(function () {
                                            var root = this;
                                            var data = $(this).data("MFLData-roomPhotoId");
                                            var rid = $(this).data("MFLData-roomId");
                                            MFLDialog("确认删除", "即将删除该照片", null, function () {
                                                MFLAjax({
                                                    action: "房间相册删除照片",
                                                    query: "&id=" + data + "&roomId=" + rid,
                                                    success: function () {
                                                        MFLHash();
                                                    }
                                                });
                                            });
                                        });
                                    }
                                });
                                callback($(".mfl-lodge-room"));
                            }
                        });
                    }
                });
            },
            filter: function (item) {
                return Number(query) == item.roomId;
            }
        });
    });

    $(".mfl-lodge-roomNewPhoto").data("MFLPageLoader", function (query, callback) {
        $(".mfl-lodge-roomNewPhoto.mfl-currentPage form .roomId").val(query);
        $(".mfl-lodge-roomNewPhoto.mfl-currentPage form").submit(function () {
            MFLAjax({
                action: "房间相册添加照片",
                query: this,
                filePosting: true,
                success: function () {
                    MFLHash(null, -1);
                }
            });
        });
        callback(".mfl-lodge-roomNewPhoto");
    });

    $(".mfl-lodge-newAttribute").data("MFLPageLoader", function (query, callback) {
        $(".mfl-lodge-newAttribute.mfl-currentPage .mfl-val-attributeIcon").lodgeIcon({
            'spirit': true,
            'cssClassImg': 'spiritIcon'
        });
        $(".mfl-lodge-newAttribute.mfl-currentPage form").submit(function () {
            var data = $(this).serialize();
            MFLAjax({
                action: "创建特性",
                query: "&" + data,
                success: function (json) {
                    MFLHash("attribute|" + json.newId, -1);
                }
            });
        });
        MFLAjax({
            action: "获取所有特性名称",
            success: function (json) {
                $(".mfl-lodge-newAttribute.mfl-currentPage form .mfl-val-attributeName").autocomplete({
                    minLength: 0,
                    delay: 0,
                    source: json.listItems
                }).focus(function () { $(this).autocomplete('search'); }).click(function () { $(this).focus(); });
                callback($(".mfl-lodge-newAttribute"));
            }
        });
    });
    $(".mfl-lodge-attribute").data("MFLPageLoader", function (query, callback) {
        MFLList({
            action: "获取特性",
            containerJ: $(".mfl-lodge-attribute.mfl-currentPage .mfl-lodge-attribute-list"),
            callback: function (json) {
                $(".mfl-lodge-attribute .mfl-lodge-title").text("特性：" + json.listItems[0].attributeName);
                $(".mfl-lodge-attribute.mfl-currentPage .mfl-list-item .mfl-val-attributeIcon").lodgeIcon({
                    'spirit': true,
                    'cssClassImg': 'spiritIcon'
                });
                $(".mfl-lodge-attribute.mfl-currentPage .mfl-list-item form.attributeForm").submit(function () {
                    var data = $(this).serialize();
                    MFLAjax({
                        action: "修改特性",
                        query: "&" + data,
                        success: function () {
                            MFLHash(null, -1);
                        }
                    });
                });
                var readOptions = function (cb) {
                    MFLList({
                        action: "获取选项",
                        query: "&id=" + query,
                        containerJ: $(".mfl-lodge-attribute.mfl-currentPage .mfl-list-item .mfl-lodge-options-list"),
                        callback: function (json) {

                            $(".mfl-lodge-attribute.mfl-currentPage .mfl-list-item .mfl-lodge-options-list .mfl-list-item .mfl-data-optionId").click(function () {
                                var root = this;
                                var data = $(this).data("MFLData-optionId");
                                MFLDialog("确认删除", "即将删除该选项", null, function () {
                                    MFLAjax({
                                        action: "删除选项",
                                        query: "&id=" + data,
                                        success: function () {
                                            readOptions();
                                        }
                                    });
                                });
                            });
                            $(".mfl-lodge-attribute.mfl-currentPage .mfl-list-item .mfl-lodge-options-list .mfl-list-item .mfl-val-optionName:last")
                            .attr('readonly', 'readonly');
                            $(".mfl-lodge-attribute.mfl-currentPage .mfl-list-item .mfl-lodge-options-list .mfl-list-item .mfl-val-optionName[value!='无']")
                            .blur(function () {
                                var btn = this;
                                if ($(this).val() == $(this).data("MFLData-optionName")) {
                                    return;
                                } //如果没修改则不提交
                                MFLSubmit(this);
                            });
                            $(".mfl-lodge-attribute.mfl-currentPage .mfl-list-item .mfl-lodge-options-list .mfl-list-item form")
                            .submit(function () {
                                var form = this;
                                var data = $(this).serialize();
                                MFLAjax({
                                    action: "修改选项",
                                    query: data,
                                    success: function (json) {
                                        var tinput = $(form).find(".mfl-data-optionName");
                                        tinput.data("MFLDataOptionName", tinput.val());
                                    },
                                    failed: function () {
                                        var tinput = $(form).find(".mfl-data-optionName");
                                        tinput.val(tinput.data("MFLDataOptionName"));
                                    }
                                });
                            });
                            if (cb) {
                                cb();
                            }
                        }
                    });
                };
                $(".mfl-lodge-attribute.mfl-currentPage .mfl-list-item .newOptionForm").submit(function () {
                    var data = $(this).serialize();
                    MFLAjax({
                        action: "创建选项",
                        query: "&" + data,
                        success: function () {
                            readOptions(function () {
                                var items = $('.mfl-lodge-attribute .mfl-list-item .mfl-lodge-options-list>.mfl-list-item');
                                $(items[items.length - 2]).find('input.mfl-val-optionName').trigger('focus')[0].select();
                            });
                        }
                    });
                });
                readOptions();

                callback($(".mfl-lodge-attribute"));
            },
            filter: function (item) {
                return Number(query) == item.attributeId;
            }
        });
    });
    $(".mfl-lodge-properties").data("MFLPageLoader", function (query, callback) {
        MFLList({
            action: "获取旅馆设定",
            containerJ: $(".mfl-lodge-properties.mfl-currentPage .mfl-propertiesList"),
            callback: function (json) {
                $(".mfl-lodge-properties.mfl-currentPage .mfl-list-item form").submit(function () {
                    var data = $(this).serialize();
                    MFLAjax({
                        action: "修改旅馆设定",
                        query: "&" + data,
                        success: function () {
                            MFLHash(null, -1);
                        }
                    });
                });
                callback($(".mfl-lodge-properties"));
            }
        });
    });
    $(".mfl-lodge-template").data("MFLPageLoader", function (query, callback) {
        MFLList({
            action: "获取旅馆设定",
            containerJ: $(".mfl-lodge-template.mfl-currentPage .mfl-templateList"),
            callback: function (json) {
                $(".mfl-lodge-template.mfl-currentPage .mfl-list-item .mfl-val-lodgeTemplateName").lodgeIcon({
                    'action': '获取模版',
                    'fieldIcon': 'templatePreviewIcon',
                    'fieldText': 'templateName',
                    'fieldValue': 'templateName',
                    'collapsable': false,
                    'diaplaySelected': false,
                    'autoSelected': true,
                    'gettingIcon': function () {
                        return '/templates/' + json.listItems[0].lodgeTemplateName + '/previewIcon.png';
                    },
                    'cssClassImg': 'templateIcon',
                    'selected': function (root) {
                        MFLList({
                            action: "获取模版",
                            containerJ: $(".mfl-lodge-template.mfl-currentPage .mfl-list-item .mfl-templatesList"),
                            callback: function (json) {
                                MFLList({
                                    action: "获取模版选项",
                                    query: {
                                        'templateName': $(root).val()
                                    },
                                    containerJ: $(".mfl-lodge-template.mfl-currentPage .mfl-list-item .mfl-templatePropertiesList"),
                                    callback: function (json) {
                                    }
                                });
                            },
                            filter: function (item) {
                                return item.templateName == $(root).val();
                            }
                        });
                    }
                });
                $(".mfl-lodge-template.mfl-currentPage .mfl-list-item form").submit(function () {
                    var data = $(this).serialize();
                    MFLAjax({
                        action: "修改旅馆设定",
                        query: "&" + data,
                        success: function () {
                            MFLHash(null, -1);
                        }
                    });
                });
                callback($(".mfl-lodge-template"));
            }
        });
    });
    $(".mfl-lodge-admin").data("MFLPageLoader", function (query, callback) {
        $(".mfl-lodge-admin.mfl-currentPage form").submit(function () {
            var data = $(this).serialize();
            MFLAjax({
                action: "设定旅馆管理员密码",
                query: "&" + data,
                success: function () {
                    MFLHash(null, -1);
                }
            });
        });
        callback($(".mfl-lodge-admin"));
    });
    $(".mfl-lodge-traffic").data("MFLPageLoader", function (query, callback) {
        MFLList({
            action: "获取旅馆设定",
            containerJ: $(".mfl-lodge-traffic.mfl-currentPage .mfl-trafficList"),
            callback: function (json) {
                $(".mfl-lodge-traffic.mfl-currentPage .mfl-list-item form.trafficForm").submit(function () {
                    var data = $(this).serialize();
                    MFLAjax({
                        action: "修改旅馆设定",
                        query: "&" + data,
                        success: function () {
                            MFLHash(null, -1);
                        }
                    });
                });
                callback($(".mfl-lodge-traffic"), function () {

                    var func = function () {
                        var map;
                        try {
                            map = new BMap.Map($(".mfl-lodge-traffic.mfl-currentPage .mfl-list-item .mapContainer")[0]);
                            mapLoaded = true;
                        }
                        catch (e) {
                            $('.mfl-lodge-traffic.mfl-currentPage .notification div').text('抱歉，地图载入失败，您暂时无法使用地图，不能选择地理位置，请确认网络正常后刷新重试。');
                            return;
                        }
                        var marker = null;
                        setTimeout(function () {
                            var point = new BMap.Point(
                                    Number($(".mfl-lodge-traffic.mfl-currentPage .mfl-list-item .mfl-val-lodgeLongtitude").val()),
                                    Number($(".mfl-lodge-traffic.mfl-currentPage .mfl-list-item .mfl-val-lodgeLatitude").val()));
                            map.addControl(new BMap.NavigationControl());               // 添加平移缩放控件
                            map.addControl(new BMap.ScaleControl());                   // 添加比例尺控件
                            map.centerAndZoom(point, 15);
                            map.enableScrollWheelZoom();
                            marker = new BMap.Marker(point);  // 创建标注
                            map.addOverlay(marker);
                            var handler = function (e) {
                                $(".mfl-lodge-traffic.mfl-currentPage .mfl-list-item .mfl-val-lodgeLongtitude").val(e.target.getCenter().lng);
                                $(".mfl-lodge-traffic.mfl-currentPage .mfl-list-item .mfl-val-lodgeLatitude").val(e.target.getCenter().lat);
                                marker.setPosition(e.target.getCenter());
                            };
                            map.addEventListener("moving", handler);
                            map.addEventListener("zoomend", handler);
                            $(".BMap_Marker").css({
                                "zindex": 1000
                            });
                            if ($(".mfl-lodge-traffic.mfl-currentPage .mfl-list-item .mfl-val-lodgeLongtitude").val() == 0 && $(".mfl-lodge-traffic.mfl-currentPage .mfl-list-item .mfl-val-lodgeLatitude").val() == 0) {
                                $(".mfl-lodge-traffic.mfl-currentPage .mfl-list-item form.mapForm").trigger("submit");
                            }
                        }, 500);
                        $(".mfl-lodge-traffic.mfl-currentPage .mfl-list-item form.mapForm input.city").val(remote_ip_info.province + " " + remote_ip_info.city + "");
                        $(".mfl-lodge-traffic.mfl-currentPage .mfl-list-item form.mapForm input.address").val(remote_ip_info.district + remote_ip_info.desc);
                        $(".mfl-lodge-traffic.mfl-currentPage .mfl-list-item form.mapForm").submit(function () {
                            var myGeo = new BMap.Geocoder();
                            // 将地址解析结果显示在地图上,并调整地图视野
                            myGeo.getPoint($(".mfl-lodge-traffic.mfl-currentPage .mfl-list-item form.mapForm input.address").val(), function (point) {
                                if (point) {
                                    map.centerAndZoom(point, 16);
                                    marker.setPosition(point);
                                } else {
                                    MFLDialog("抱歉", "没有在地图数据库中查找到您输入的地址。")
                                }
                            }, $(".mfl-lodge-traffic.mfl-currentPage .mfl-list-item form.mapForm input.city").val());
                        });
                    }
                    if (mapLoaded) {
                        func();
                    } else {
                        $('<link rel="stylesheet" type="text/css" href="http://api.map.baidu.com/res/12/bmap.css"/>').appendTo('head');
                        $.ajax({
                            url: 'http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=js',
                            dataType: 'script'
                        });
                        $.ajax({
                            url: 'http://api.map.baidu.com/getscript?v=1.2',
                            dataType: 'script',
                            success: function () {
                                if (timer) {
                                    clearTimeout(timer);
                                    func();
                                }
                            }
                        });
                        var timer = setTimeout(func, 5000);
                    }
                });
            }
        });
    });

    $(".mfl-lodge-roomPhoto").data("MFLPageLoader", function (query, callback) {
        var pid = query.split("|")[0];
        var roomId = query.split("|")[1];
        MFLList({
            action: "房间相册获取照片",
            query: "&roomId=" + roomId,
            containerJ: $(".mfl-lodge-roomPhoto.mfl-currentPage .mfl-roomPhotoList"),
            callback: function (json) {
                $(".mfl-lodge-roomPhoto .mfl-lodge-title").text("照片：" + json.listItems[0].roomPhotoTitle);
                $(".mfl-lodge-roomPhoto.mfl-currentPage .mfl-list-item form").submit(function () {
                    var data = $(this).serialize();
                    MFLAjax({
                        action: "房间相册修改照片",
                        query: "roomId=" + roomId + "&" + data,
                        success: function () {
                            MFLHash(null, -1);
                        }
                    });
                });
                callback($(".mfl-lodge-roomPhoto"));

            },
            filter: function (item) {
                return Number(pid) == item.roomPhotoId;
            }
        });
    });
    $(".mfl-lodge-newRoom").data("MFLPageLoader", function (query, callback) {
        $(".mfl-lodge-newRoom.mfl-currentPage .mfl-val-roomIcon").lodgeIcon();
        $(".mfl-lodge-newRoom.mfl-currentPage form").submit(function () {
            var data = $(this).serialize();
            MFLAjax({
                action: "创建房间",
                query: "&" + data,
                success: function (json) {
                    MFLHash("room|" + json.newId, -1);
                }
            });
        });
        callback($(".mfl-lodge-newRoom"));
    });


    $(".mfl-lodge-lodgePhoto").data("MFLPageLoader", function (query, callback) {
        MFLList({
            action: "旅馆相册获取照片",
            containerJ: $(".mfl-lodge-lodgePhoto.mfl-currentPage .mfl-lodgePhotoList"),
            callback: function (json) {
                MFLAjax({
                    action: "获取所有相册分类",
                    success: function (json2) {
                        $(".mfl-lodge-lodgePhoto .mfl-lodge-title").text("照片：" + json.listItems[0].lodgePhotoTitle);
                        $(".mfl-lodge-lodgePhoto.mfl-currentPage .mfl-list-item form").submit(function () {
                            var data = $(this).serialize();
                            MFLAjax({
                                action: "旅馆相册修改照片",
                                query: "&" + data,
                                success: function () {
                                    MFLHash(null, -1);
                                }
                            });
                        });
                        $(".mfl-lodge-lodgePhoto.mfl-currentPage .mfl-list-item form .mfl-val-lodgePhotoType").autocomplete({
                            minLength: 0,
                            source: json2.lodgePhotoTypes,
                            autoFocus: true,
                            delay: 0
                        }).focus(function () {
                            $(this).autocomplete('search', '');
                        }).click(function () {
                            $(this).focus();
                        });
                        callback($(".mfl-lodge-lodgePhoto"));
                    }
                });
            },
            filter: function (item) {
                return item.lodgePhotoId == Number(query);
            }
        });
    });
    $(".mfl-lodge-lodgeNewPhoto").data("MFLPageLoader", function (query, callback) {
        MFLAjax({
            action: "获取所有相册分类",
            success: function (json2) {
                $(".mfl-lodge-lodgeNewPhoto.mfl-currentPage form").submit(function () {
                    MFLAjax({
                        action: "旅馆相册添加照片",
                        query: this,
                        filePosting: true,
                        success: function (json) {
                            MFLHash(null, -1);
                        }
                    });
                });
                $(".mfl-lodge-lodgeNewPhoto.mfl-currentPage form .mfl-val-lodgePhotoType").autocomplete({
                    minLength: 0,
                    source: json2.lodgePhotoTypes,
                    autoFocus: true,
                    delay: 0
                }).focus(function () {
                    $(this).autocomplete('search', '');
                }).click(function () {
                    $(this).focus();
                }); ;
                callback($(".mfl-lodge-lodgeNewPhoto"));
            }
        });
    });
});