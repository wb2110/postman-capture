using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CefSharp.WinForms.Example
{
    class FIRequestHandler:CefSharp.Handler.RequestHandler
    {
        private BrowserForm form;
        public FIRequestHandler(BrowserForm form) {
            this.form = form;
        }
        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            /*
             * {
			"name": "单个资产计提折旧",
			"request": {
				"method": "PUT",
				"header": [
					{
						"key": "Content-Type",
						"name": "Content-Type",
						"type": "text",
						"value": "application/json"
					},
					{
						"key": "X-CAF-Runtime-Context",
						"type": "text",
						"value": "{{X-CAF-Runtime-Context}}"
					}
				],
				"body": {
					"mode": "raw",
					"raw": "{\n\t\"beanName\":\"com.inspur.gs.fi.aa.termend.api.accuredeprecication.api.AccrueDepreciationService\",\n\t\"methodName\":\"SingleAssetDepreciation\",\n\t\"formalTypeList\":[\"java.lang.String\",\"java.lang.String\",\"java.lang.String\",\"java.lang.String\",\"java.math.BigDecimal\"],\n\t\"paramTypeList\":[\"java.lang.String\",\"java.lang.String\",\"java.lang.String\",\"java.lang.String\",\"java.math.BigDecimal\"],\n\t\"paramList\":[\"e6b7f585-9c8f-495d-9fdb-8be943de1134\",\"2020\",\"04\",\"7e42069a-037d-4b94-b6ec-e39ff75f7f3c\",5000]\n\t\n}",
					"options": {
						"raw": {
							"language": "json"
						}
					}
				},
				"url": {
					"raw": "{{baseUrl}}/api/bf/df/v1.0/fipub/fiapitestservice/testbean",
					"host": [
						"{{baseUrl}}"
					],
					"path": [
						"api",
						"bf",
						"df",
						"v1.0",
						"fipub",
						"fiapitestservice",
						"testbean"
					]
				}
			},
			"response": []
		}*/
            if (request.Url.Contains("/api/fi")==false) {
                return null;
            }
            var bstart = form.bStart;
            if (bstart) {
                var box=new mybox(request.Url);
                bool bCancel = false;
                string reqName = "";
                box.onClose += (sender, e) =>
                {
                    reqName = ((MyBoxEvent)e).text;
                    bCancel = ((MyBoxEvent)e).bCanel;
                };
                box.ShowDialog();
                if (bCancel) {
                    return null;
                }

                var arr = (JArray)form.postman["item"];
                var item = new JObject();
                item["name"] = reqName;
                arr.Add(item);

                var req = new JObject();
                item["request"] = req;
                req["method"] = request.Method;

                //头部
                var headers = new JArray();
                req["header"] = headers;
                foreach (var h in request.Headers) {
                    var header = new JObject();
                    header["key"] = h.ToString();
                    header["value"] = request.Headers.GetValues(h.ToString())[0];
                    header["type"] = "text";
                    headers.Add(header);
                }

                //body
                var body = new JObject();
                req["body"] = body;
                body["mode"] = "raw";
                var postData = request.PostData;
                if (postData != null)
                {
                    var elements = postData.Elements;

                    var charSet = request.GetCharSet();

                    foreach (var element in elements)
                    {
                        if (element.Type == PostDataElementType.Bytes)
                        {
                            var data = element.GetBody(charSet);
                            body["raw"] = data;
                        }
                    }
                }

               
                var options = JObject.Parse("{\"raw\":{\"language\":\"json\"}}");
                body["options"] = options;



                //path
                var urlArr = request.Url.Split('/');
                var host = "{{baseUrl}}"; // string.Join("/", urlArr.Take(3));

                var url = new JObject();
                req["url"] = url;
                url["raw"] = "{{baseUrl}}" + string.Join("/", urlArr.Skip(3).ToArray());   //request.Url;
                var hosts = new JArray();
                hosts.Add(host);
                url["host"] = hosts;
                var paths = new JArray();
                url["path"]= JArray.FromObject( urlArr.Skip(3).ToArray());

            }
            //Default behaviour, url will be loaded normally.
            return null;
        }
    }
}
