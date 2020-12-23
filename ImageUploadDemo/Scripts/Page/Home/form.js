new Vue({
    el: "#app",
    data: function () {
        return {
            form: {
                name: null
            },
            images: []
        }
    },
    methods: {
        submit() {
            // 表單
            let form = ($('form'))[0];
            let formData = new FormData(form);
            for (let key in this.form) {
                if (this.form.hasOwnProperty(key))
                    formData.append(key, this.form[key]);
            }
            // 檔案
            let files = this.$refs.filesInfo.files
            console.log(files)
            this.CreateFormData(formData, 'files', files)
        },
        CreateFormData(formData, key, data) {
            if (data === Object(data) || Array.isArray(data)) {
                for (const i in data) {
                    this.CreateFormData(formData, key + '[' + i + ']', data[i]);
                }
            } else {
                formData.append(key, data);
            }
        },
        // 選擇圖片上傳dialog onchange事件
        filesChangeHandler(e) {
            console.log(e.target.files)
            this.images = e.target.files
        },
        displayUploadImage(img) {
            this.showImage(img, $("#preview"))
        },
        showImage(file, $img) {
            const reader = new FileReader();
            reader.onload = function (e) {
                $img.attr("src", e.target.result)
            }
            reader.readAsDataURL(file)
        },

        CheckPasteUpload(e) {
            console.log(e)
            //偵測剪貼簿裡是否有圖檔，沒有的話就結束，有的話存在 file
            let file = null;
            for (let i = 0; i < e.clipboardData.items.length; i++) {
                const item = e.clipboardData.items[i];
                if (item.type.indexOf("image") !== -1) {
                    file = item.getAsFile();
                    break;
                }
            }
            if (file === null) {
                return;
            }

            console.log(file)
            this.showImage(file, $("#img1"))


            const formData = new FormData();
            formData.append('pasteFile', file, "filename"); //filename不能為空字串
        }
    }
})