using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MFLJson;
namespace MFLAdmin.Lodge
{
    public partial class GetTestJson : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            JsonObject root;
            switch (Request.QueryString["test"])
            {
                case "lodgeList":
                    root = Json.Object(
                        "listItems",Json.Array(
                            Json.Object(
                                "lodgeName", Json.String("STR"),
                                "P2",Json.String("STR"),
                                "P3",Json.String("STR"),
                                "P4",Json.String("STR"),
                                "P5",Json.String("STR")
                            )
                        )
                        );
                    break;
                default:
                    root = Json.Object(
                        "message",Json.String("不存在这项测试的数据。"),
                        "success",Json.False()
                        );
                    break;
            }
            root["success"] = Json.True();
            Response.Write((root as IJson).ToString());
        }
    }
}