
$( document ).ready( function() {

    var count = 0;
    var percentComplete = 0;
    var upfiles = 0;


    $('#drop-area').bind('dragover', function (event) {
        event.stopPropagation();
        event.preventDefault();
    });

    $('#drop-area').bind('drop', function (event) {

        event.stopPropagation();
        event.preventDefault();
 
        var image = event.originalEvent.dataTransfer.files;
        var img = document.createElement("img");
        img.classList.add("obj");
        img.file = image[0];

        var reader = new FileReader();
        reader.onload = (function (aImg) { return function (e) { aImg.src = e.target.result; }; })(img);
        reader.readAsDataURL(image[0]);

        reader.addEventListener("load", function () {
            var im = new Image();
            im.title = image[0].name;
            im.src = this.result;
            $('#drop-area').css('background-image', 'url(' + im.src + ')');
            $('#drop-area').css('background-size', '100% 100%');
            $('#drop-area').attr('src', im.src);
        }, false);



        if( upfiles == 0 )
            upfiles = event.originalEvent.dataTransfer.files;
        else {
            if( confirm( "Drop: Do you want to clear files selected already?" ) == true ) {
                upfiles = event.originalEvent.dataTransfer.files;
                $( '#fileToUpload' ).val('');
            }
            else
                return;
        }
        $( "#fileToUpload" ).trigger( 'change' );
    });

    $("#upload_btn").click( function() {
        var arr = [];
        $(".bootstrap-tagsinput span").each(function (index, elem) {
           
            if ($(this)[0].className == 'tag label label-info')
            {
             
                arr.push($(this).text())
            }
        });

 
        var data = new FormData();
        var file = upfiles[0];
        data.append('file', file);
        data.append('name', $('#filenamefield').val());
        data.append('tags', arr.join());
        data.append('description', $('#descriptionfield').val());      
        uploadFormData(data);
    });


  

    $( "#fileToUpload" ).change( function() {
   
        var fileIn = $( "#fileToUpload" )[0];

        if (!upfiles) {
            upfiles = fileIn.files;
            onloadFile();
        }
        else {
            if (fileIn.files.length > 0) {
                if (confirm("Drop: Do you want to clear files selected already?") == true) {
                    upfiles = fileIn.files;
                    onloadFile();
                }

                else
                    return;
            }

        }

        $('#drop-area').html('<b> Selected Files </b><br />');
        $('#upload_btn').prop("disabled", false);

        $(upfiles).each( function(index, file) {
            size = Math.round( file.size / 1024 );
            if( size > 1024 )
                size =  Math.round( size / 1024 ) + ' mb';
            else
                size = size + ' kb';
            $('#drop-area').append(file.name + " ( " + size + " ) <br />");
        });

    });




    function onloadFile() {
        var file = document.querySelector('input[type=file]').files[0];
        if (/\.(jpe?g|png|gif)$/i.test(file.name)) {
            var reader = new FileReader();

            reader.addEventListener("load", function () {
              
                var im = new Image();
                im.title = file.name;
                im.src = this.result;
                $('#drop-area').css('background-image', 'url(' + im.src + ')');
                $('#drop-area').css('background-size', '100% 100%');
                $('#drop-area').attr('src', im.src);

            }, false);
            reader.readAsDataURL(file);

        }

    }

    function uploadFormData(formData) {
        
        $.ajax({
            url: "UploadFiles",
            type: "POST",
            data: formData,
            contentType: false,
            cache: false,
            processData: false,
            success: function (data) {
                $('#drop-area').append(data);
            }
        });
    }
    $(document).on('click', 'span', function (e) {

        $(this).parent().remove()

    });


    $("#add_tag_in_input").click(function () {

        var checker=true

        $(".bootstrap-tagsinput").children().each(function (i, elm) {
            if($(this).text() == $('#inputtagfield').val())
            {
                checker=false
            }
        });


        if ($('#inputtagfield').val() != "" && checker!=false)
            $('.bootstrap-tagsinput').prepend(' <span  class="tag label label-info">' + $('#inputtagfield').val() + '<span data-role="remove"></span></span>')
    });
    //////////////////////////////////////////////////
    /////////////////////////////////////////////////

    $(document).ready(function () {
        $.ajax({
            url: 'GetAllMyGallery',
            type: "GET",
            async: true,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (response) {
                var list = response;
                $.each(list, function (index, item) {
                    $('#fullPreview').after(
        ' <li> <a href="/Image/' + item.filename + '"></a> <img data-original="/Image/' + item.filename + '" src="/Image/' + item.filename + '" width="240" height="150" alt="Ocean" /><div class="overLayer"></div><div class="infoLayer">' +
                               '<ul><li><h2>'+item.name+'</h2></li><li><p>View more</p></li></ul></div><div class="projectInfo">'+ item.description+'</div></li>'
                );                      
                });
                $('#gallery').least();   

                $('#fullPreview').after(
'<li id="buttadd">' +
'<img id="addimagebutton" style="cursor: pointer; display: inline;"  data-original="/image/add.png" src="/image/add.png" alt="Ocean" width="240" height="150">' +
'</li>')                  
                resize();
            },

            error: function (error) {
                $(this).remove();
                DisplayError(error.statusText);
            }
        });
    });

    function resize()
    {
     
        $("body").css('transform', 'scale(2,2)');
        $(document).scrollTop(-1000);
        $(document).scrollTop(1000);
        $("body").css('transform', 'scale(1,1)');
        if ($('#gallery').height() <= 650) {
            $('#gallery').css('height', '650')
        }
    }
    $(document).on('click', '.tagname', function (e) {
        $.ajax({
            url: "GetAllMyGalleryByTag",
            type: "GET",
            async: true,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data:{"tagName":e.currentTarget.textContent},
            success: function (response) {
                var list = response;
                $('#gallery').empty();
                $('#gallery').append('<li id="fullPreview"></li>')
                $.each(list, function (index, item) {                 

                    $('#fullPreview').after(
        ' <li> <a href="/Image/' + item.filename + '"></a> <img data-original="/Image/' + item.filename + '" src="/Image/' + item.filename + '" width="240" height="150" alt="Ocean" /><div class="overLayer"></div><div class="infoLayer">' +
                               '<ul><li><h2>' + item.name + '</h2></li><li><p>' + item.tagname + '</p></li></ul></div><div class="projectInfo">' + item.description + '</div></li>'
                );
                });
                      
                $('#gallery').least();  

                $('#fullPreview').after(
             '<li id="buttadd">' +
             '<img id="addimagebutton" style="cursor: pointer; display: inline;"  data-original="/image/add.png" src="/image/add.png" alt="Ocean" width="240" height="150">' +
             '</li>')
                resize();

            }
                 
        });
              
        resize();
              
    });

    $(window).load(function () {
      
        $.ajax({
            url: '/api/InteticsTestAppAPI',
            async: true,
            dataType: 'json',

            success: function (data) {
                var str = '<div class="tagdiv">'
                str += '<p class="tagtext">e.g.</p> '
                str += '<p class="tagname">' + data[1] +  '</p> '
                str += '<p class="tagname">'  + data[2] +  '</p> '
                str += '<p class="tagname">' + data[3] +  '</p> '
                str += '<p class="tagname">'  + data[4] +  '</p> '
                str += '</div>'
                $('#search-form').after(str)
            }
        });

    });
    $(document).ready(function () {
        var win = $(window);

 
        win.scroll(function () {


            if ($(document).height() - win.height() == win.scrollTop()) {

                $.ajax({
                    url: 'index',
                    dataType: 'html',
                    success: function (html) {
                        $('#gallery').append(html);
                
                    }
                });
            }
        });
    });  
    $(document).on('click', '.submit-container', function (e) {
        $.ajax({
            url: "GetAllMyGalleryByTag",
            type: "GET",
            async: true,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { "tagName": $('.search-field').val()  },
            success: function (response) {
                var list = response;
                $('#gallery').empty();
                $('#gallery').append('<li id="fullPreview"></li>')
                $.each(list, function (index, item) {

                    $('#fullPreview').after(
        ' <li> <a href="/Image/' + item.filename + '"></a> <img data-original="/Image/' + item.filename + '" src="/Image/' + item.filename + '" width="240" height="150" alt="Ocean" /><div class="overLayer"></div><div class="infoLayer">' +
                               '<ul><li><h2>' + item.name + '</h2></li><li><p>' + item.tagname + '</p></li></ul></div><div class="projectInfo">' + item.description + '</div></li>'
                );
                });

                $('#gallery').least();

                $('#fullPreview').after(
             '<li id="buttadd">' +
             '<img id="addimagebutton" style="cursor: pointer; display: inline;"  data-original="/image/add.png" src="/image/add.png" alt="Ocean" width="240" height="150">' +
             '</li>')
                resize();
            }
        });
    })

    $('.search-field').keypress(function (event) {

        $("p").remove(".serchhelp");        
        $.ajax({
            url: "SearchTags",
            type: "GET",
            async: true,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: { "tagName": ($('.search-field').val() + event.key).toLocaleLowerCase() },
            success: function (response) {
                var list = response;
                var thehtml='';
                $.each(list, function (index, item) {
                    thehtml += '<p class="serchhelp" id="outputcontent" onclick="$(' + "'" + '.search-field' + "'" + ').val(' + "'" + item + "'" + ')" style="cursor:pointer"><strong style="cursor:pointer" >' + item + ' </strong> <br> <br> </p> ';
                 
                })
                if (thehtml != '') {
                    $('#outputbox').html(thehtml);
                }
            }
        });
    });

    document.addEventListener("click", function (event) {
        if(event.target.className!='serchhelp' ||'search-field'!=event.target.className)
        {
            $("p").remove(".serchhelp");
        }
    });
    $(document).ready(function () {
        $(document).on('click', '#addimagebutton', function (event)
        { 
            event.preventDefault(); 
            $('#overlay').fadeIn(400, 
                function () { 
                    $('#modal_form')
                        .css('display', 'block') 
                        .animate({ opacity: 1, top: '40%' }, 200);
              
                  
                
                });
         
        });
               
        $('#modal_close, #overlay').click(function () {
            $('#modal_form')
                .animate({ opacity: 0, top: '45%' }, 200,  
                    function () {
                          
                        $('#modal_form').css('display', 'none'); 
                        $('#overlay').fadeOut(400);
                    }
                );
        });
    });

         
    

});

