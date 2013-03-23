/// <reference path="/ISPReferences/Dev/Actions.htm.isp.js">
/*<!--*/
$load('Default.htm.master.js')({
    body: function () {
    /*-->
    <div class="hero-unit">
    所有的Action均为数据更改时要调用的接口，该种接口有两种调用方式：ajax和表单提交
    </div>
    
    <div class="hero-unit"><h2>Ajax</h2>
    <h4>请求方式：</h4>
    使用jQuery的ajax方法并且需要注明dateType为json，否则服务器会默认返回可以在浏览器里查看的html结果。另外，服务器返回的json也不一定是严格的json格式（特别是数据量比较大的情况下），最好指定ajax的converters为
    <pre>
    {
        'text json':function(data){
            return eval('a='+data);
        }
    }</pre>
    <h4>错误处理：</h4>
    进行ajax请求时发生错误，会在服务器端返回500错误，并且返回ContentType为text/plain的错误消息(直接浏览器调试时也可以看到)。使用jQuery.ajax方法可指定error的值对错误进行处理:
    <pre>
    'error':function(jqXHR, textStatus, errorThrown){
        if(jqXHR.responseText){
            alert(jqXHR.responseText);//操作错误
        }else{
            //网络错误
        }
    }</pre>
    </div>
    <div class="hero-unit"><h2>表单提交</h2>
    <h4>请求方式：</h4>
    任意接口可直接使用表单进行post或者get提交，但需要在接口后注明redirect参数，以便在操作成功后进行跳转，redirect参数的相对路径以接口url的路径为基准，除非redirect为空字符串，在redirect为空字符串时将跳转回调用接口的页面。
    <br />例如在/a.htm调用
    <pre>
    /Hello/Say?redirect=b.htm       会跳转至/Hello/b.htm
    /Hello/Say?redirect=../b.htm    会跳转至/b.htm
    /Hello/Say?redirect=            会跳转至/a.htm
    /Hello/Say?                     不会跳转，仍然输出参数的json格式</pre>
    <h4>错误处理：</h4>
    将会跳转到错误页面（在本站中，将会记录错误信息到Cookie，然后跳转会调用页面，然后在/Auth/GetStatus接口中会将错误信息输出）
    </div>

    <!--*/
    }
});
//-->