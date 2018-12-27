$(document).ready(function () {

    var $tagEditor = $(".post-tag-editor");

    $tagEditor
    .find(".tag-select")
    .on("click", "> li> a", function (e) {

        e.preventDefault();

        var $this = $(this);
        var $tagParent = $this.parent();
        $tagParent.toggleClass("selected");
      

        var selected = $tagParent.hasClass("selected");
        $tagParent.find(".selected-input").val(selected);
    });

    var $addTagBtn = $tagEditor.find(".add-tag-btn");
    var $newTagName = $tagEditor.find(".new-tag-name");

    $addTagBtn.click(function (e) {

        e.preventDefault();
        addTag($newTagName.val());
    });
    $newTagName
    .keyup(function () {

        if ($newTagName.val().trim().length > 0) {
            $addTagBtn.prop("disabled", false);
        } else {
            $addTagBtn.prop("disabled", true);
        }
    })
    .keydown(function (e) {

        if (e.which != 13) {
            return;
        } else {
            e.preventDefault();
            addTag($newTagName.val());
        }
    });
     
    function addTag(name) {
        var newIndex = $tagEditor.find(".tag-select > li").length - 1;

        $tagEditor
        .find(".tag-select > li.template")
        .clone()
        .removeClass("template")
        .addClass("selected")
        .find(".name").text(name).end()
        .find(".name-input").val(name).attr("name", "Tags[" + newIndex + "].Name").end()
        .find(".selected-input").attr("name", "Tags[" + newIndex + "].IsChecked").val(true).end()
        .appendTo($tagEditor.find(".tag-select"));

        $newTagName.val("");
        $addTagBtn.prop("disabled", true);

    }
});