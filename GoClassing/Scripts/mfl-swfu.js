(function ($) {
    $.fn.mflSwfUpload = function (o) {
        var roots = this;
        MFLAjax({
            action: "GetVariables",
            //loadingJ:ui_s,
            success: function (j) {
                $(roots).each(function () {
                    o = $.extend({
                        types: "swf",
                        text: "选取文件",
                        uploadBtnId: "",
                        ajaxOptions: new Object(),
                        callback: function () { }
                    }, o);
                    o.root = this;
                    var root = this;
                    o.id = $(o.root).attr("id");
                    var swfu;
                    if (!o.id) {
                        var i = 0;
                        while ($("#mflSwfUpload" + i).length) {
                            i++;
                        }
                        $(this).attr("id", o.id = "#mflSwfUpload" + i);
                    }
                    var loading = $('<div><p>正在开启上传通道：</p><img src="/Styles/loading.gif" alt="请稍候" /></div>').appendTo(o.root).hide();
                    var ui_b = $('<div id="' + o.id + '_browseBtn"></div>').appendTo(o.root);
                    var ui_p = $('<div id="' + o.id + '_progressBar"></div>').appendTo(o.root).css({ position: 'relative', zIndex: '10001' }).progressbar().hide();
                    var ui_s = $('<div id="' + o.id + '_status" class="ui-widget-content"><span class="text"></span><br/></div>').css({ position: 'relative', zIndex: '10001' }).appendTo(o.root).hide();
                    var ui_c = $('<div>取消</div>').appendTo(ui_s).button().click(function () {
                        swfu.cancelUpload();
                    });
                    ui_s.children(".text").html("上传速度：" + "<br/>已用时间：" + "<br/>剩余时间：" + "<br/>已上传：" + "<br/>总共：");
                    var success = o.ajaxOptions.success;
                    var json;
                    var overlay;
                    $(this).data("mflSwfUploadOptions", o);
                    var settings = {
                        flash_url: j.repositoryUrl + "Flash/swfupload.swf",
                        flash9_url: j.repositoryUrl + "Flash/swfupload_fp9.swf",
                        upload_url: "",
                        post_params: new Object(),
                        file_size_limit: "2 GB",
                        file_types: j.swfuTypes[o.types],
                        file_types_description: j.swfuTypesDescription[o.types],
                        file_upload_limit: 0,
                        file_queue_limit: 1,
                        debug: false,
                        // Button settings
                        button_image_url: "/Styles/TestImageNoText_65x29.png",
                        button_width: "200",
                        button_height: "33",
                        button_placeholder_id: o.id + '_browseBtn',
                        button_text: '<span class="theFont">' + o.text + '</span>',
                        button_text_style: ".theFont { font-size: 14; }",
                        button_text_left_padding: 12,
                        button_text_top_padding: 5,

                        // The event handler functions are defined in handlers.js
                        swfupload_preload_handler: function () {
                            if (!this.support.loading) {
                                MFLDialog("抱歉", "您需要安装 Flash Player 9.028 或者更高版本才可以上传内容.", function () { });
                                return false;
                            }
                        },
                        swfupload_load_failed_handler: function () {
                            try {
                                overlay.remove();
                            }
                            catch (e) { }
                            MFLDialog("抱歉", "加载上传控件时发生了一个错误，请重试.", function () { });
                        },
                        file_queued_handler: function (file) {
                            //            try {
                            //                var progress = new FileProgress(file, this.customSettings.progressTarget);
                            //                progress.setStatus("Pending...");
                            //                progress.toggleCancel(true, this);

                            //            } catch (ex) {
                            //                this.debug(ex);
                            //            }

                        },
                        file_queue_error_handler: function (file, errorCode, message) {
                            var alert = function (msg) {
                                try {
                                    overlay.remove();
                                }
                                catch (e) { }
                                MFLDialog("上传时发生错误", "抱歉:" + msg);
                            };
                            try {
                                if (errorCode === SWFUpload.QUEUE_ERROR.QUEUE_LIMIT_EXCEEDED) {
                                    alert("您选择的文件个数太多，请选择一个文件。");
                                    return;
                                }
                                switch (errorCode) {
                                    case SWFUpload.QUEUE_ERROR.FILE_EXCEEDS_SIZE_LIMIT:
                                        alert("您选择的文件过大.");
                                        break;
                                    case SWFUpload.QUEUE_ERROR.ZERO_BYTE_FILE:
                                        alert("您不能上传大小为0的文件或者大小超过2G的文件.");
                                        break;
                                    case SWFUpload.QUEUE_ERROR.INVALID_FILETYPE:
                                        alert("不支持的文件类型.");
                                        break;
                                    default:
                                        if (file !== null) {
                                            alert("发生了一个未处理的错误");
                                        }
                                        break;
                                }
                            } catch (ex) {
                            }
                        },
                        file_dialog_complete_handler: function (numFilesSelected, numFilesQueued) {
                            if (numFilesQueued > 0) {
                                var t = this;
                                if (!o.ajaxOptions.query) {
                                    o.ajaxOptions.query = new Object();
                                }
                                o.ajaxOptions.query["selectedFilename"] = t.getQueueFile(0).name;
                                o.ajaxOptions.success = function (j) {
                                    var s = t.getQueueFile(0).size;
                                    if (s > j.uploadLimit) {
                                        MFLDialog("文件过大", "抱歉，您选择的文件过大(" + s + ")，该文件类型最大允许上传" + SWFUpload.speed.formatBytes(j.uploadLimit));
                                        t.cancelUpload();
                                        return;
                                    }
                                    t.setUploadURL(j.uploadApUrl + '&FileSize=' + s);
                                    setTimeout(function () {
                                        ui_p.show();
                                        ui_s.slideDown(function () {

                                            t.startUpload();
                                            overlay = $('<div></div>').addClass('ui-widget-overlay').appendTo('body').css({ height: $('body').height() > $(window).height() ? $('body').height() : $(window).height(), zIndex: 10000 });
                                        });
                                    }, 100);
                                    json = j;
                                };
                                o.ajaxOptions.failed = function (msg) {
                                    t.cancelUpload(null, false);
                                    MFLNotify(msg);
                                };
                                loading.show();
                                o.ajaxOptions.complete = function () {
                                    loading.hide();
                                };

                                MFLAjax(o.ajaxOptions);
                            }
                        },
                        upload_start_handler: function (file) {
                            return true;
                        },
                        upload_progress_handler: function (file, bytesLoaded, bytesTotal) {
                            ui_p.progressbar("value", bytesLoaded * 100 / bytesTotal);
                            ui_s.children('.text').html("正在上传：" + file.name
                                + "<br/><span class=\"floatright\">" + SWFUpload.speed.formatBPS(file.currentSpeed) + "</span>上传速度："
                                + "<br/><span class=\"floatright\">" + SWFUpload.speed.formatTime(file.timeElapsed) + "</span>已用时间："
                                + "<br/><span class=\"floatright\">" + SWFUpload.speed.formatTime(file.timeRemaining) + "</span>剩余时间："
                                + "<br/><span class=\"floatright\">" + SWFUpload.speed.formatBytes(bytesLoaded) + "</span>已上传："
                                + "<br/><span class=\"floatright\">" + SWFUpload.speed.formatBytes(bytesTotal) + "</span>总共：");
                        },
                        upload_error_handler: function (file, errorCode, message) {
                            ui_p.hide();
                            ui_s.slideUp();
                            try {
                                overlay.remove();
                            }
                            catch (e) { }
                            json = null;
                            try {
                                var alert = function (msg) {
                                    MFLDialog("上传时发生错误", "抱歉:" + msg);
                                };
                                switch (errorCode) {
                                    case SWFUpload.UPLOAD_ERROR.HTTP_ERROR:
                                        alert("上传时发生错误: " + message);
                                        break;
                                    case SWFUpload.UPLOAD_ERROR.UPLOAD_FAILED:
                                        alert("上传失败.");
                                        break;
                                    case SWFUpload.UPLOAD_ERROR.IO_ERROR:
                                        alert("网络连接错误或者服务器暂时不可用");
                                        break;
                                    case SWFUpload.UPLOAD_ERROR.SECURITY_ERROR:
                                        alert("安全错误");

                                        break;
                                    case SWFUpload.UPLOAD_ERROR.UPLOAD_LIMIT_EXCEEDED:
                                        alert("超出上传大小限制.");
                                        break;
                                    case SWFUpload.UPLOAD_ERROR.FILE_VALIDATION_FAILED:
                                        alert("Failed Validation.  Upload skipped.");
                                        break;
                                    case SWFUpload.UPLOAD_ERROR.FILE_CANCELLED:
                                        //                        // If there aren't any files left (they were all cancelled) disable the cancel button
                                        //                        if (this.getStats().files_queued === 0) {
                                        //                            document.getElementById(this.customSettings.cancelButtonId).disabled = true;
                                        //                        }
                                        //                        progress.setStatus("Cancelled");
                                        //                        progress.setCancelled();
                                        break;
                                    case SWFUpload.UPLOAD_ERROR.UPLOAD_STOPPED:
                                        //                        progress.setStatus("Stopped");
                                        break;
                                    default:
                                        alert("未处理的错误，代码: " + errorCode);
                                        //                        this.debug("Error Code: " + errorCode + ", File name: " + file.name + ", File size: " + file.size + ", Message: " + message);
                                        break;
                                }
                            } catch (ex) {
                            }
                        },
                        upload_success_handler: function (file, serverData) {


                        },
                        upload_complete_handler: function (file) {
                            if (this.getStats().files_queued > 0) {
                            }
                        },
                        queue_complete_handler: function (numFilesUploaded) {
                            if (json) {
                                success(json);
                            }
                            $(root).children("object").height(29);
                            ui_p.hide();
                            ui_s.slideUp();
                            try {
                                overlay.remove();
                            }
                            catch (e) { }

                        },
                        swfupload_loaded_handler: function () {
                            $.proxy(o.callback, swfu)();
                        }
                        // Queue plugin event
                    };
                    swfu = new SWFUpload(settings);
                    gSwfu = swfu;
                });
            }
        });
    };
})(jQuery);
var gSwfu;