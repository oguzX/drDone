$('.dropify').dropify({
        messages: {
            'default': 'Fotoğrafı buraya sürükleyin yada tıklayın',
            'replace': 'Fotoğrafı değiştirmek için Fotoğrafı buraya sürükleyin yada tıklayın',
            'remove': 'Temizle',
            'error': 'Bir sorun oluştu.',
            'imageFormat': 'Bu formatta dosya yükleyemezsiniz. Kabul edilen formatlar ({{ value }}).'
        }
});
$(function () {
    $('img[data-toggle=popover]').popover({
        html: true,
        trigger: 'hover',
        content: function () {
            return '<img src="' + $(this).attr('src') + '" />';
        }
    });
    $('[data-toggle="popover"]').popover();
    $(".alert").delay(5000).fadeOut(1000);
    $("#open-addCatModal").on('click', function () {
        $('#modal_category_add').modal({
            show:true
        });
        $("#addButton").on("click", function () {
            var url = "/product/addCategory";
            var categoryName = $("#CategoryName").val().trim();
            $(this).addClass("disabled");
            $.ajax({
                url: url,
                type: "Post",
                dataType: "json",
                data: { _categoryname: categoryName },
                success: function (data) {
                    console.log(data);
                    var type = ["warning","success"]
                    var forAdd = '<div class="alert alert-'+type[data.result]+'" style="display:none" role="alert" ><b>' + data.message + '!</b> </div>';
                    $("#result").html(forAdd);
                    $("#result .alert").fadeIn(500).delay(1000).fadeOut(500);
                    if (data.result == 1) {
                        var categoryCount = parseInt($("#allCategories > li").length);
                        var forAppend = '<li class="list-group-item"><input id="Categories_' + categoryCount + '__Id" name="Categories[' + categoryCount + '].Id" type="hidden" value="' + data.id + '">' +
                            '<label for="Roles_' + categoryCount+ '__IsChecked">' +
                            '<input id="Categories_' + categoryCount+'__IsChecked" name="Categories['+categoryCount+'].IsChecked" type="checkbox" value="true"><input name="Categories[' +categoryCount+ '].IsChecked" type="hidden" value="false"> ' + categoryName + '</label></li>';
                        $("#allCategories").append(forAppend);
                        $("#CategoryName").val("");
                    } else {
                        $("#addButton").removeClass("disabled");
                    }
                }
            })
        });
    });
});