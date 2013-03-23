<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MyFamilyLodge.Default1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="header" class="masthead">
		<div class="inner">
			<div class="container">

				<div class="row">
					<div class="span8 intro">
						<h1>我的家庭旅馆！</h1>
						<p>&nbsp;</p>
						<p>不需要租用服务器，</p>
						<p>不需要购买网站程序，不需要技术人员协助，</p>
						<p>只需要轻松点击几下，即可免费获得您的家庭旅馆网站。</p>
						<a href="/Membership/" class="btn large success intro-btn">立刻免费获得！</a>
						<%--<span>             OR             </span>
						<a href="" class="btn large success intro-btn">V I P 会 员 增 值 计 划</a>--%>
					</div>
					<div class="span8 introvideo">

						<ul class="pills">
							<li class="active"><a href="">旅馆模板</a></li>
							<li><a href="">旅馆展示</a></li>
							<li><a href="">使用帮助</a></li>
						</ul>
						<img class="thumbnail" src="http://placehold.it/500x300" alt=""/>

					</div>
				</div>
			</div>
		</div>
	</div>
	<div id="featured">
		<div class="container">
			<div class="row">
				<div class="span-one-third">

					<span><img src="images/icon/template_90x90.png" alt=""/></span>
					<div>
						<h3 class="featured-title">个性化的模板</h2>
						专业设计团队针对家庭旅馆<br/>
						量身订做的精美模板<br/>
					</div>
				</div>

				<div class="span-one-third">
					<span><img src="images/icon/config_90x90.png" alt=""/></span>
					<div>
						<h3 class="featured-title">简单易用的操作</h2>
						只需要简单上传照片，文字描述<br/>
						网站设计配置一键搞定<br/>
					</div>

				</div>
				<div class="span-one-third">
					<span><img src="images/icon/booking_90x90.png" alt=""/></span>
					<div>
						<h3 class="featured-title">专业的预订管理</h2>
						客房对比、查询、预订管理<br/>
						实时显示客房预订状态<br/>

					</div>
				</div>
			</div>
		</div><!-- END OF CONTAINER -->
	</div><!-- END OF FEATURED -->
	<div id="client">
		<div class="container">
			<div class="row">
				<div id="client-testimonial" class="span8">

					<h2 class="testimonial-title">客户评价</h2>
					<ul id="commentary_users" class="unstyled">
						<li><a href=""><img src="http://placehold.it/120x120" alt="Katy Bairstow" width="120" height="120" /></a>
						<div class="blockquote">
						<p>The bottom line for us, is the time saving. FreeAgent saves us about four hours a week in tedious accounting practices, whilst giving us better quality </p>
						<cite><a href="">Katy Bairstow</a> | <span class="context">Mojo Media</span></cite>

						</div>
						</li>
						<li><a href=""><img src="http://placehold.it/120x120" alt="Katy Bairstow" width="120" height="120" /></a>
						<div class="blockquote">
						<p>The bottom line for us, is the time saving. FreeAgent saves us about four hours a week in tedious accounting practices, whilst giving us better quality </p>
						<cite><a href="">Katy Bairstow</a> | <span class="context">Mojo Media</span></cite>

						</div>
						</li>
					</ul>
				</div><!-- END OF CLIENT TESTIMONIAL-->
				<div id="client-show" class="span8">
					<h2 class="show-title">客户展示</h2>
						<ul class="media-grid">
						  <li>

							<a href="#">
							  <img class="thumbnail" src="http://placehold.it/120x120" alt="">
							</a>
						  </li>
						  <li>
							<a href="#">
							  <img class="thumbnail" src="http://placehold.it/120x120" alt="">
							</a>
						  </li>

						  <li>
							<a href="#">
							  <img class="thumbnail" src="http://placehold.it/120x120" alt="">
							</a>
						  </li>
						  			
						  <li>

							<a href="#">
							  <img class="thumbnail" src="http://placehold.it/120x120" alt="">
							</a>
						  </li>
						  <li>
							<a href="#">
							  <img class="thumbnail" src="http://placehold.it/120x120" alt="">
							</a>
						  </li>

						  <li>
							<a href="#">
							  <img class="thumbnail" src="http://placehold.it/120x120" alt="">
							</a>
						  </li>			  
						</ul>

				</div><!-- END OF CLIENT SHOW-->
			</div>
		</div><!-- END OF CONTAINER-->
	</div><!--  END OF CLIENT -->
</asp:Content>
