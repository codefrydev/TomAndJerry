window.loadDisqus = function (pageUrl, pageIdentifier) {
    var disqus_config = function () {
        this.page.url = pageUrl;  // Replace PAGE_URL with your page's canonical URL variable
        this.page.identifier = pageIdentifier; // Replace PAGE_IDENTIFIER with your page's unique identifier variable
    };

    (function () {
        var d = document, s = d.createElement('script');
        s.src = 'https://tomandjerry-2.disqus.com/embed.js';
        s.setAttribute('data-timestamp', +new Date());
        (d.head || d.body).appendChild(s);
    })();
}
