using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoClassing.Internal
{
    public class UI
    {
        public static string LoginTimeout="登陆超时";
        public static string AlreadyInCourse="您已经加入了该课程，或者您已经申请了加入该课程但是尚未被批准加入。";
        public static string PagePrev="上一页";
        public static string PageNext = "下一页";
        public static string MustChooseAValidCtype1 = "必须选择一个有效的课程所属类型";
        public static string VerificationHashWrong = "抱歉，该链接不正确，请确定您从收到的邮件中打开了完整的链接。";
        public static string TagAlreadyInCourse = "分类“{0}”已经存在于课程中。";
        public static string AlreadyApplyingJoinCourse = "您已经申请了加入该课程，请等待主讲批准，";
        public static string LoggingViaHttp = "您正在使用普通Http协议登陆上课网，推荐您使用安全套接字(HTTPS)协议访问上课网，您可以点击确定继续使用普通Http协议登陆，也可以点击下行链接切换到安全套接字(HTTPS)协议：<br/><a href=\"https://www.goclassing.com\">https://www.Goclassing.com/</a>";



        public static string ConfirmDeleteNote = "您确定要删除这条留言吗？";


        public static string MailVerification = "邮箱验证";
        public static string MailNeedVerification = "一封邮件已经发送到了您的电子邮箱，请打开邮件中附带的链接以验证您的电子邮箱。很抱歉，在验证您的电子邮箱之前，我们无法让您登陆。";
        public static string MailVerificationContent = "尊敬的{1}，您好！<br />欢迎您在上课网注册，这是一封电子邮件地址验证邮件，请点击下面的链接以验证您的电子邮件地址，如果地址不能够被点击，请复制其到浏览器地址栏打开：<br /><a href=\"{0}\">{0}</a>";
        public static string MailPwdRetrievalContent = "尊敬的{1}，您好！<br />您申请了重设您的登录密码，您的密码已经重设为：{0}<br />您也可以点击打开以下链接直接登录并修改新密码：<br/><a href=\"http://test.goclassing.com:7777/?QuickLogin=true&Username={1}&Password={0}&ReturnUrl=%2FHome%2FSecurity%2F%3FOldPassword%3D{0}%23Password\">http://test.goclassing.com:7777/?QuickLogin=true&Username={1}&Password={0}&ReturnUrl=%2FHome%2FSecurity%2F%3FOldPassword%3D{0}%23Password</a><br />如果该次重设密码操作不是您的操作，请立即联系我们的管理员。谢谢。";
        public static string MailPwdRetrieval="取回密码";
        public static string MailVerificated = "邮箱验证成功";
        public static string MailVerificatedContent = "尊敬的{0}，恭喜！<br />您的电子邮箱（{1}）已经验证成功，赶快登陆上课网吧。";



        public static string NotAllowGuestViewProfile = "该用户的设置不允许访客查看其个人资料";
        public static string NotAllowGuestViewMembers = "该课程的设置不允许访客查看其成员列表";
        public static string NotAllowGuestViewPostList = "当前该分类下的资料不允许访客查看";
        public static string NotAllowGuestViewCourses = "该用户的设置不允许访客查看其教授的课程列表";
        public static string NotAllowGuestViewPaticipatedCourses = "该用户的设置不允许访客查看其正在学习的课程列表";
        public static string NotAllowAddFriend = "该用户的设置不接收陌生人的好友请求";
        public static string NotAllowGuestViewFriends = "该用户的设置不允许访客查看其好友列表";
        public static string NotAllowGuestJoin = "该课程当前设置不允许提交加入申请。";
        public static string NotAllowReply = "当前该分类下的资料不允许学员回复，或内容尚未准备好。";
        public static string NotAllowGuestViewNotes = "该用户的设置不允许访客查看其留言列表";



        public static string FeedsWelcomeToGoClassing = "欢迎来到上课网，你可以试着<a href=\"/C/All/\">搜索你感兴趣的课程和分类</a>，还可以<a href=\"/Home/Courses/?createCourse=true\">创建自己的课程</a>以上传丰富的多媒体内容。";
        public static string FeedsNewVideo = "<a href=\"/C/{3}/{4}\">{0}在课堂{1}内容“{2}”中发布了一段视频<br/><img src=\"/Drop/Reply{5}.mp4.jpg\"></a>";
        public static string FeedsNewAudio = "<a href=\"/C/{3}/{4}\">{0}在课堂{1}内容“{2}”中发布了一段音频</a>";
        public static string FeedsNewDoc = "<a href=\"/C/{3}/{4}\">{0}在课堂{1}内容“{2}”中发布了一个Word文档</a>";
        public static string FeedsNewPdf = "<a href=\"/C/{3}/{4}\">{0}在课堂{1}内容“{2}”中发布了一个Pdf文档</a>";
        public static string FeedsNewXls = "<a href=\"/C/{3}/{4}\">{0}在课堂{1}内容“{2}”中发布了一个电子表格</a>";
        public static string FeedsNewPpt = "<a href=\"/C/{3}/{4}\">{0}在课堂{1}内容“{2}”中发布了一个演示文稿</a>";


        public static string MsgNewJoinApply = "{0}申请了加入你的课程{1}。";
        public static string MsgFriendCreateCourse = "你的好友{0}创建了课程{1}，赶快去看一看吧。";
        public static string MsgRemovedMembership = "{0}删除了你在{1}的学员身份。";
        public static string MsgDeniedJoin = "{0}拒绝了你加入{1}的申请。";
        public static string MsgApprovedJoin = "{0}同意了你加入{1}的申请。";
        public static string MsgFriendAccepted = "{0}同意了你的添加好友申请。";
        public static string MsgFriendSalutting = "{0}申请添加你为好友。";
        public static string MsgFriendDenied = "{0}拒绝了你的添加好友申请。";
        public static string MsgNewReplyInPost = "{0}在{1}中回复了你。";
        public static string MsgNewNote = "{0}给你留言了。";
        public static string MsgNewNoteReply = "{0}回复了你的留言。";
        public static string MsgTruenameNotConfig="您尚未填写您的真实姓名，填写真实姓名与所在地，可以更方便地在上课网享受教与学的乐趣。";
        public static string MsgPwdAnserNotConfig="您尚未设置密码安全问题与答案，为了您的账户安全，建议您及时设置。";
        
        public UI(HttpContext context)
        {
        }

    }
}