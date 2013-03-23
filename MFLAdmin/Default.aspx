<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MFLAdmin.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">

    $(function () {
        $('.topbar-inner form').show();
    });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%
    var rooms = this.MFLAdmin.GetRoomsInfo();
    var countPeople = 0;
    var countEmpty = 0;
    foreach (var room in rooms)
    {
        var state = this.MFLAdmin.GetRoomState(this.Day, room.Id);
        countPeople += state.People;
        if (!state.Full)
        {
            countEmpty++;
        }
        
     %>
				<!-- list-item-start -->
                
				<div class="room-list-item <%=state.Full?"":"available" %> clearfix">
				<span class="roomName"><%=room.Name %><br /><span class="statusNum">入住人数：<%=state.People %></span></span>
				<span class="list-roomAttributes">
                <%foreach (var attribute in room.Attributes)
                  {%>
                  
					<span><img title="<%=attribute.Name %>:<%=attribute.OptionName %>" style="<%=attribute.Icon %>" src="/Style/blank.gif" class="roomAttributesIcon" /><span class="optionName"><%=attribute.OptionName %></span></span>
                    <%}%>
				</span>
				<span class="edit">
					<a href="" title="编辑" class="edit"><img src="/Style/lodge/pencil.png"></a>
					<a href="#" title="查看" onclick="return false;" class="more"><img src="/Style/lodge/search.png"></a>
				</span>
				</div>
	<div class="dialog" title="房间入住情况-<%=room.Name %>-<%=Day.ToShortDateString() %>">
		<form action="Default.aspx" id="form1" method="post">
			<div class="status">
				<span>
					<input <%=state.Full?"checked":"" %> type="radio" value="full" name="state" />
					<label for="">已入住</label>
				</span>
				<span>
					<input <%=!state.Full?"checked":"" %> type="radio" value="empty" name="state" />
					<label for="">未入住</label>
				</span>
			</div>
			<br />
            <input type="hidden" name="year" value="<%=Day.Year %>" />
            <input type="hidden" name="month" value="<%=Day.Month %>" />
            <input type="hidden" name="day" value="<%=Day.Day %>" />
            <input type="hidden" name="roomId" value="<%=room.Id %>" />
			<div class="clearfix">
				<label for="people">入住人数</label>
				<input readonly="readonly" type="text" name="people" value="<%=state.People %>" class="amount"/>
				<div class="slider"></div>
			</div>
			<label for="contact">联系人</label>
			<input type="text" name="contact" value="<%=state.Contact %>" class="state"/><br />

			<label for="tel">联系电话</label>
			<input type="text" name="tel" value="<%=state.Tel %>" class="state" /><br />
			<label for="qq">QQ</label>
			<input type="text" name="qq" value="<%=state.QQ %>" class="state" /><br />
			<label for="email">Email</label>
			<input type="text" name="email" value="<%=state.Email %>" class="state" /><br />
			<label for="memo">备注</label>
			<textarea name="memo" cols="30" class="state" rows="5"><%=state.Memo %></textarea>
		</form>
	</div>
				<!-- list-item-end -->
<% 
    }
    (this.Master as MFLAdmin.Site).CountRoom = (rooms.Length-countEmpty).ToString();
    (this.Master as MFLAdmin.Site).CountEmpty = countEmpty.ToString();
    (this.Master as MFLAdmin.Site).CountPeople = countPeople.ToString();
    %>
</asp:Content>
