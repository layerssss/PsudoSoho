<%@ Page Title="旅馆列表" Language="C#" MasterPageFile="Frame.Master" AutoEventWireup="true" CodeBehind="Lodges.aspx.cs" Inherits="MyFamilyLodge.Account.Lodges" %>
<%@ Import Namespace="MFL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
<script src="../js/easyTooltip.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container">
	<div class="accountWrapper">
		<h1>旅馆列表</h1>
		<table width="100%">
		<thead>
		<tr>
		<td>旅馆类型</td>
		<td>旅馆状态</td>
		<td>到期时间</td>
		<td>旅馆名称</td>
		<td>配置面板</td>
		<td>管理中心地址</td>
		<td>展示地址</td>
		<td>可进行的操作</td>
		</tr>
		</thead>
		<tbody>
		<%foreach (var product in this.Account.MFLUsers_Account.MFLUsers_product)
		  {
			  %>
			  <tr><%
			  var lodge = product.MFL_lodge.FirstOrDefault();
			  if (lodge != null)
			  {
			  %>
			  <td><%= MFLAccount.GetProductType(product.type)%></td>
			  <td>旅馆状态：正在使用中</td>
			  <td><%=product.due_time.ToShortDateString() %></td>
			  <td><%=this.GetLodgeName(lodge)%></td>
			  <td><a target="_blank" href="<%=SharedConfig.AdminBaseUrl %>Lodge/?lodge=<%=Server.UrlEncode(lodge.ident) %>#root">进入</a></td>
			  <td><a class="tooltip" style="white-space:nowrap;overflow:hidden;display:block;" target="_blank" href="<%=SharedConfig.AdminBaseUrl %><%=lodge.ident %>/" title="<%=SharedConfig.AdminBaseUrl %><%=lodge.ident %>/">进入</a></td>
			  <td><a class="tooltip" style="white-space:nowrap;overflow:hidden;display:block;" target="_blank" href="<%=SharedConfig.LodgeBaseUrl %><%=lodge.ident %>/" title="<%=SharedConfig.LodgeBaseUrl %><%=lodge.ident %>/">进入</a></td>
			  <td><form method="post">
			  <input type="hidden" name="productId" value="<%=product.id %>" />
			  <input class="btn info" type="submit" value="续费" name="submit" />
			  <input class="btn danger" type="submit" value="反激活" name="submit" />
			  </form></td>
			<%}
			  else
			  {
			  %>
			  <td><%= MFLAccount.GetProductType(product.type)%></td>
			  <td>未激活</td>
			  <td><%=product.due_time.ToShortDateString() %></td>
			  <td>不可用</td>
			  <td>激活后可用</td>
			  <td width=100 style="width=100px;">激活后可用</td>
			  <td width=100 style="width=100px;">激活后可用</td>
			  <td><form method="post">
			  <input type="hidden" name="productId" value="<%=product.id %>" />
			  <input class="btn success" type="submit" value="激活" name="submit" />
			  <input class="btn info" type="submit" value="续费" name="submit" />
			  <input class="btn danger" type="submit" value="申请退款" name="submit" />
			  </form></td>
			  <%
			  }
			  %></tr><%
		  }
		  
		  %></tbody></table>
		  <a href="/Prices.aspx" class="btn primary">添置新旅馆</a>
	  </div>
</div>  
	  
</asp:Content>
