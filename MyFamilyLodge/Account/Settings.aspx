<%@ Page Title="账户设定" Language="C#" MasterPageFile="Frame.Master" AutoEventWireup="true"
    CodeBehind="Settings.aspx.cs" Inherits="MyFamilyLodge.Account.Settings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
    <link href="<%=MFL.SharedConfig.AdminBaseUrl %>Style/custom-theme/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="<%=MFL.SharedConfig.AdminBaseUrl %>Scripts/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var data = new Object();
    </script>
    <script type="text/javascript">
        data.pProvince = '<%=MFLUsers_Account.pProvince %>';
        data.pCity = '<%=MFLUsers_Account.pCity %>';
    </script>
    <script type="text/javascript">
        $(function () {
            $('.accordion').accordion({
				active: 0,
				animated: false,//"bounceslide",
                collapsable: true,
                navigation: true,
                change: function (event, ui) {
                    location.hash = $(ui.newHeader).children('a').attr('href');
                }
            });
            var pca = PCA();
            for (var i in pca) {
                $('select[name="pProvince"]').append($('<option></option>', {
                    val: pca[i].p,
                    selected: pca[i].p == data.pProvince
                }).text(pca[i].p).data('cities', pca[i].c));
            }
            $('select[name="pProvince"]').change(function () {
                $('select[name="pCity"] option').remove();
                $('select[name="pCity"]').append($('<option val="">未选择</option>'))
                var cities = $(this).children('option:selected').data('cities');
                for (var j in cities) {
                    $('select[name="pCity"]').append($('<option></option>', {
                        val: cities[j],
                        selected: cities[j] == data.pCity
                    }).text(cities[j]));
                }
            }).change();
        });
    </script>
    <script src="../js/PCA.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container">
	<div class="accountWrapper">
		<h1>账户设定</h1>
		<div class="accordion">
			<h2><a href="#Password">修改密码</a></h2>
			<div>
				<form action="#Password" method="post">
				<table>
					<tr>
						<td>
							当前密码：
						</td>
						<td>
							<input type="password" name="passwordOld" />
						</td>
					</tr>
					<tr>
						<td>
							新密码：
						</td>
						<td>
							<input type="password" name="password" />
						</td>
					</tr>
					<tr>
						<td>
							重复新密码：
						</td>
						<td>
							<input type="password" name="password2" />
						</td>
					</tr>
					<tr>
						<td colspan="2">
							<input class="btn primary" type="submit" name="submit" value="修改密码" />
						</td>
					</tr>
				</table>
				</form>
			</div>
			<h2>
				<a href="#Profiles">修改资料</a></h2>
			<div>
				<form action="#Profiles" method="post">
				<table>
					<tr>
						<td>
							称呼：
						</td>
						<td>
							<input type="text" name="pName" value="<%=MFLUsers_Account.pName %>" />
							<input type="radio" name="pSex" <%=MFLUsers_Account.pSex?"checked=\"\"":"" %> value="true" />先生
							<input type="radio" name="pSex" <%=!MFLUsers_Account.pSex?"checked=\"\"":"" %> value="false" />女士
						</td>
					</tr>
					<tr>
						<td>
							省份：
						</td>
						<td>
							<select name="pProvince" >
							<option value="">未选择</option>
							</select>
						</td>
					</tr>
					<tr>
						<td>
							城市：
						</td>
						<td>
							<select name="pCity">
								<option value="">未选择</option>
							</select>
						</td>
					</tr>
					<tr>
						<td>
							每月大约客流量：
						</td>
						<td>
							<select name="pTraffic">
							<option value="50">&lt;50</option>
							<option value="100">50~100</option>
							<option value="300">100~300</option>
							<option value="600">300~600</option>
							<option value="2000">&gt;600</option>
							</select>
						</td>
					</tr>
					<tr>
						<td colspan="2">
							<input class="btn primary" type="submit" name="submit" value="修改资料" />
						</td>
					</tr>
				</table>
				</form>
			</div>
		</div>
	</div>
</div>
	
</asp:Content>
