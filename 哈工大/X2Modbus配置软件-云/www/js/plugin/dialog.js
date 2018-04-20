function GetRequest() {
    var url = location.search; //获取url中"?"符后的字串
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        var strs = str.split("&");
        for(var i = 0; i < strs.length; i ++) {
            theRequest[strs[i].split("=")[0]]=unescape(strs[i].split("=")[1]);
        }
    }
    return theRequest;
}


var dialog = {
    alert:function (content) {
        easyDialog.open({
            fixed:true,
            overlay:false,
            container:{
                header: $.i18n('tips'),
                content:'<div class="text-center">'+content+"</div>" ,
                yesFn:function () {
                },
                noFn:false
            }
        });
        $(".prompt_input").focus();
        return easyDialog;
    },
    prompt:function (header, success) {
        easyDialog.open({
            fixed:true,
            overlay:false,
            container:{
                header:header || "&nbsp;",
                content:'<input type="text" class="form-control prompt_input" placeholder="value"/> ',
                yesFn:function () {
                    success($(".prompt_input").val());
                },
                noFn:true
            }
        });
        $(".prompt_input").focus();
        return easyDialog;
    },
    boolean:function(data){
        data = $.extend({
            trueFn:$.noop,
            falseFn:$.noop,
            title:"&nbsp;",
            status:"0"
        },data);
        easyDialog.open({
            overlay:false,
            container:{
                header:data.title || "&nbsp;",
                content:'当前状态:'+data.status,
                yesText:"开",
                noText:"关",
                yesFn:function () {
                    data.trueFn();
                },
                noFn:function(){
                    data.falseFn();
                }
            }
        });
        return easyDialog;
    },
    openWindow:function(data){
        data = $.extend({
            url:"",
            left:0,
            top:0,
            width:200,
            height:200,
            overlay:false,
            header:"&nbsp;",
            follow:$("body")[0]
        },data);
        var content = '<div>' +
            '<iframe src="http://baidu.com" ' +
            'style="border:0;padding:0;margin:0;overflow:hidden;height:100%;width:100%;"></iframe></div>';
        var body = $("body");
        if(data.width<1){
            data.width *= body.width();
        }
        if(data.height < 1){
            data.height *= body.height();
        }
        var $content = $(content).css({width:data.width||200, height:data.height||200});
        var temp = $("<div></div>").append($content);
        easyDialog.open({
            overlay :data.overlay,
            follow:data.follow,
            followX : data.left,
            followY : data.top,
            container:{
                header:data.header||"&nbsp;",
                content:temp.html()
            }
        });
        return easyDialog;
    }

};