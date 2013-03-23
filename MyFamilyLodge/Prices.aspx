<%@ Page Title="产品价格" Language="C#" MasterPageFile="~/Page.master" AutoEventWireup="true" CodeBehind="Prices.aspx.cs" Inherits="MyFamilyLodge.Prices" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
<script type="text/javascript">
    var activeCol = function (i) {
        i++;
        $('.price-details-tb td:nth-child(' + i + '),.price-details-tb th::nth-child(' + i + ')').addClass('activeProduct').siblings().removeClass('activeProduct');
        var lastT = null;
        var afunc = function () {
            var t = $(window).scrollTop();
            var a = 400;
            t += (a - t) * 0.1;
            t = Math.round(t);
            if (lastT!=t) {
                lastT = t;
                $(window).scrollTop(t);
                setTimeout(afunc, 10);
            }
        };
        afunc();
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHeader" runat="server">
	<h2 id="price-header-s">产品价格</h2>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div id="price-content">
		<div class="container">
			<div class="Product-active Product">
				<div class="ProductHeader">
				  <h1 class="ProductName">寻旅.筑家</h1>
				  <p>我们为您提供了3种不同的解决方案<br />您可以根据需要选择最适合您的一种。</p>
				  <p>原来建立自己的家庭旅馆网站这么简单！</p>
				  <div class="extended">
					<p><a href="/Help/">了解更多 →</a></p>
				  </div>
				</div>
				<ol class="ProductPlans clearfix">
			          <li class="ProductPlan">
			            <h2 class="plan">超级连锁旅</h2>
			            <h3>
			              <span class="amount">即将推出</span><span class="duration"></span>
			            </h3>
			            <div class="extended">
			              <ul class="features">
			                <li>同时用于多家旅馆！</li>
			                <li>超大相册空间！</li>
			                <li>不限制旅馆房间数量！</li>
			              </ul>
			              <a href="#" onclick="activeCol(1);return false;" class="btn primary">详 细 资 料</a>
			            </div>
			          </li>
			          <li class="ProductPlan">
			            <h2 class="plan">白金旅</h2>
			            <h3>
			              <span class="amount">即将推出</span><span class="duration"></span>
			            </h3>
			            <div class="extended">
			              <ul class="features">
			                <li>绑定自己的域名！</li>
			                <li>可绑定淘宝网链接！</li>
			                <li>可订制高级模板</li>
			              </ul>
			              <a href="#" onclick="activeCol(2);return false;" class="btn primary">详 细 资 料</a>
			            </div>
			          </li>
			          <li class="ProductPlan">
			            <h2 class="plan">迷你旅</h2>
			            <h3>
			              <span class="amount">￥0.00！免费！</span>
			            </h3>
			            <div class="extended">
			              <ul class="features">
			                <li>只需点击几下，即可配置完成！</li>
			                <li>包括MFL的全部基础功能！</li>
			                <li>可使用所有的基础模版！</li>
			              </ul>
			              <a href="#" onclick="activeCol(3);return false;" class="btn primary">详 细 资 料</a>
			            </div>
			          </li>
			        </ol>
			</div><!-- END FOR Price Plan -->
			<div id="price-details">
				<table class="price-details-tb zebra-striped">
					<thead>
					  <tr>
						<th></th>
						<th>超级连锁旅</th>
						<th>白金旅</th>
						<th>迷你旅</th>
					  </tr>
					</thead>
					<tbody>
					  <tr>
						<td>价格</td>
						<td>暂时不可用</td>
						<td>暂时不可用</td>
						<td>永久免费！</td>
					  </tr>
					  <tr>
						<td></td>
						<td></td>
						<td></td>
						<td><a class="btn" href="/Account/Wizards/AddProduct.aspx?type=free">添加到我的账户</a></td>
					  </tr>
					  <tr>
						<td>数目限制</td>
						<td>20/每旅馆</td>
						<td>20/每旅馆</td>
						<td>10/每旅馆</td>
					  </tr>	
					  <tr>
						<td>旅馆数目限制</td>
						<td>3/每账户</td>
						<td>1/每账户</td>
						<td>1/每账户</td>
					  </tr>
					  <tr>
						<td>房间数目限制</td>
						<td>无限制！</td>
						<td>20/每旅馆</td>
						<td>10/每旅馆</td>
					  </tr>
					  <tr>
						<td>绑定自己的域名</td>
						<td>支持*</td>
						<td>支持</td>
						<td></td>
					  </tr>
					  <tr>
						<td>相册容量</td>
						<td>1GB/每旅馆</td>
						<td>500MB/每旅馆</td>
						<td>100MB/每旅馆</td>
					  </tr>
					  <tr>
						<td>基础模版随意使用</td>
						<td>可用</td>
						<td>可用</td>
						<td>可用</td>
					  </tr>	
					  <tr>
						<td>高级模版</td>
						<td>可用</td>
						<td>可用</td>
						<td></td>
					  </tr>
					  <tr>
						<td>客房预订在线查询与管理</td>
						<td>支持</td>
						<td>支持</td>
						<td>支持</td>
					  </tr>
					  <tr>
						<td>自定义房间特性</td>
						<td>支持</td>
						<td>支持</td>
						<td>支持</td>
					  </tr>
					  <tr>
						<td>房间相册介绍</td>
						<td>支持</td>
						<td>支持</td>
						<td>支持</td>
					  </tr>
					  <tr>
						<td>旅馆整体相册</td>
						<td>支持</td>
						<td>支持</td>
						<td>支持</td>
					  </tr>			  
					</tbody>
				</table>
                <div class="notification attention">
                注释：*同一个账户下的超级连锁旅只允许绑定到一个域名
                </div>
			</div><!-- END FOR Price Details -->
		</div><!-- END FOR Container -->
	</div><!-- END FOR Price Content -->
</asp:Content>
