(function(){
    var defaultLan = 'zh-cn';
    var $lanList = $('#lanList');
    $lanList.on('click','li', function(){
        var $li = $(this);
        var lan = $li.attr('data-lan');
        setLanguage(lan);
        window.location.reload();
    });
    var i18n = $.i18n();

    function createLanList(cb){
        $.getJSON('/i18n/config.json', function(conf){
            var li = '';
            for(var i=0;i<conf.length;i++){
                var item = conf[i];
                li += '<li data-lan="'+item.value+'"><a href="javascript:;">'+item.name+'</a></li>';
                if(item["default"]){
                    defaultLan =  item.value;
                }
            }
            $lanList.html(li);
            cb();
        });
    }
    function updateText(lan) {
        lan = lan || getLanguage();
        setLanguage(lan);
        i18n.locale = lan ;
        i18n.load('i18n/' + i18n.locale + '.json', i18n.locale)
            .done(function () {
                $('html').i18n();
                $('.invisible').removeClass('invisible').css('opacity', 0).animate({'opacity':1},200);
            });

    }

    function getLanguage(){
        return $.cookie('language') || defaultLan || 'zh-cn';
    }
    function setLanguage(lan){
        $.cookie('language', lan, {expires:365});
    }
    $(document).ready(function ($) {
        createLanList(function(){
            updateText();
        });
    });
})();
