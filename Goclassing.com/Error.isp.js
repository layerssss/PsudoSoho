/*<!--*/
$load('master/frame.master.js')({
    title: 'Oops',
    head: function () { /*--><title>Message</title><!--*/ },
    body: function () {
        
        /*-->
        <script type="text/javascript">
            $(function(){
                var s = location.search;
                s = s.substring(s.lastIndexOf('message=') + 'message='.length);
                if(s.indexOf('&')!=-1){
                s = s.substring(0, s.indexOf('&'));
                }

                $('#myModal .modal-body').text(String(decodeURIComponent(s).replace(/\+/g, ' ')));
                $('#myModal').modal('show').on('hidden', function () {
                    history.go(-1);
                })
                //
            });
        </script>
        <!--*/
    }
});
//-->