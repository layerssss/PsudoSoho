<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MasonryGallery.Uploader.Default" %>
<!DOCTYPE html>
<html>
<head runat="server">
<title>SWFUpload Powered Uploader</title>
    <link href="/Uploader/default.css" rel="stylesheet" type="text/css" />
<script src="/Uploader/swfupload/swfupload.js" type="text/javascript"></script>
<script src="/Uploader/swfupload/swfupload.queue.js" type="text/javascript"></script>
<script src="/Uploader/swfupload/swfupload.cookies.js" type="text/javascript"></script>
<script src="/Uploader/js/fileprogress.js" type="text/javascript"></script>
<script src="/Uploader/js/handlers.js" type="text/javascript"></script>
<script type="text/javascript">

    var postParams = new Object();
    var onlyOne = true;
</script>
<script class="donotoptimize" type="text/javascript">
    
<%
    if (Request.QueryString["url"] != null)
    {
        %>
        postParams.url = '<%=Request.QueryString["url"].Replace("'","") %>';
        onlyOne=true;
        <%
    }
     %>
</script>
<script type="text/javascript">
    var swfu;
    window.onload = function () {
        var settings = {
            flash_url: "swfupload/swfupload.swf",
            flash9_url: "swfupload/swfupload_fp9.swf",
            upload_url: "http://masonrygallery.com/Uploader/",
            post_params: postParams,
            file_size_limit: "20 MB",
            file_types: "*.jpg;*.png;*.bmp;*.tif;",
            file_types_description: "All Files",
            file_upload_limit: 100,
            file_queue_limit: onlyOne ? 1 : 0,
            custom_settings: {
                progressTarget: "fsUploadProgress",
                cancelButtonId: "btnCancel"
            },
            debug: false,

            // Button settings
            button_image_url: "images/TestImageNoText_65x29.png",
            button_width: "80",
            button_height: "29",
            button_placeholder_id: "spanButtonPlaceHolder",
            button_text: '<span class="theFont">Browse</span>',
            button_text_style: ".theFont { font-size: 16; }",
            button_text_left_padding: 12,
            button_text_top_padding: 3,

            // The event handler functions are defined in handlers.js
            swfupload_preload_handler: preLoad,
            swfupload_load_failed_handler: loadFailed,
            file_queued_handler: fileQueued,
            file_queue_error_handler: fileQueueError,
            file_dialog_complete_handler: fileDialogComplete,
            upload_start_handler: uploadStart,
            upload_progress_handler: uploadProgress,
            upload_error_handler: uploadError,
            upload_success_handler: onlyOne ? (function () {
                parent.$.colorbox.close();
            }) : uploadSuccess,
            upload_complete_handler: uploadComplete,
            queue_complete_handler: queueComplete	// Queue plugin event
        };

        swfu = new SWFUpload(settings);
    };
	</script>
</head>
<body>
<br /><br /><br /><br />
<div id="content">
	<form id="form1" action="Default.aspx" method="post" enctype="multipart/form-data">
			<div class="fieldset flash" id="fsUploadProgress">
			<span class="legend"><%
                                     if (Request.QueryString["url"] != null)
                                     {
        %>
        New photos to &quot;<%=Request.QueryString["title"]%>&quot;
        <%
                                     }
                                     else
                                     {
                                         %>Choose one picture to be the cover of the new album.<%
                                     }
     %></span>
			</div>
		<div id="divStatus">0 Files Uploaded</div>
			<div>
				<span id="spanButtonPlaceHolder"></span>
				<input id="btnCancel" type="button" value="Cancel All Uploads" onclick="swfu.cancelQueue();" disabled="disabled" style="margin-left: 2px; font-size: 8pt; height: 29px;" />
			</div>
	</form>
    <div>Powered by SWFUpload</div>
</div>
</body>
</html>
