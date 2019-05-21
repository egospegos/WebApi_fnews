/*jshint esversion: 6 */
const uri = "/api/topics/";

const uri2 = "/api/account/Register";
const urilike = "/api/like/";

let items = null;
let globalTop;
let globalNov;
let isAdm;
var Role = "";
var RoleID = "";

document.addEventListener("DOMContentLoaded", function (event) {
    getTopics();
    document.getElementById("loginBtn").addEventListener("click", logIn);
    document.getElementById("logoffBtn").addEventListener("click", logOff);
    getCurrentUser();
});

//установить айди темы
function setTopic(id) {
    globalTop = id;
}

//установить айди новости
function setNovelty(id) {
    globalNov = id;
}

//получить количество тем
function getCount(data) {
    const el = document.querySelector("#counter");
    let name = "Количество тем: ";
    if (data > 0) {
        el.innerText = name + data;
    } else {
        el.innerText = "Тематик еще нет";
    }
}

//вывести все темы и новости к ним
function getTopics() {
    GetRole();
    this.isAdmin()
        .then(
        response => {
            isAdm = response.message;
                let request = new XMLHttpRequest();
                request.open("GET", uri);
                request.onload = function () {
                    let topics = "";
                    let topicsHTML = "";
                    topics = JSON.parse(request.responseText);

                    //Кнопка добавления темы
                    if (Role == "admin") {
                       

                        topicsHTML += '<div class="myb fixed-bottom d-flex justify-content-end">';
                        topicsHTML += '<button style="outline: none;  background:transparent; border: 0;" data-toggle="modal" data-target="#exampleModal">';
                        topicsHTML += '<i class="fas fa-plus-circle fa-3x"></i>';
                        topicsHTML += '</button>';

                        topicsHTML += '</div>';
                                               
                    }
                   

                    if (typeof topics !== "undefined") {
                        getCount(topics.length);
                        if (topics.length > 0) {
                            if (topics) {
                                var i;
                                for (i in topics) {
                                    topicsHTML += '<div class="p-3">'
                                    topicsHTML += '<div class="card border-dark"> <div class="card-header topicText">';


                                    topicsHTML += '<div style="font-size: 21px;" class="d-flex justify-content-between">';
                                    topicsHTML += topics[i].name;
                                    
                                   //кнопки редактирования и удаления темы только дял админа
                                    if (isAdm == "admin") {
                                        topicsHTML += '<div>';
                                        
                                        topicsHTML += '<button style="outline: none; background: transparent; border: 0;" data-toggle="modal" data-target="#modalChange" onclick="editTopic(' + topics[i].topicId + ')"><i class="fas fa-pen-square fa-lg"></i></button>';
                                        topicsHTML += '<button class="close ml-2" aria-label="Close" onclick="deleteTopic(' + topics[i].topicId + ')"> <span aria-hidden="true">&times;</span> </button>';
                                       
                                        topicsHTML += '</div>';
                                        
                                    }
                                    topicsHTML += '</div>';                                    
                                    topicsHTML += '</div>';                                    
                                   
                                 

                                    topicsHTML += '<div class="card-body">';
                                    topicsHTML += '<div><span>' + ' </span>';

                                    if (typeof topics[i].novelty !== "undefined" && topics[i].novelty.length > 0) {
                                        let j;
                                        for (j in topics[i].novelty) {
                                            topicsHTML += '<div class="d-flex justify-content-between">';
                                            topicsHTML += '<p class="card-text font-weight-bold font-italic">' + topics[i].novelty[j].title + "</p>";
                                            topicsHTML += '<div class="align-top">';

                                            //кнопка лайка
                                        //    topicsHTML += "<button type='button' class='btn btn-link ml-auto user userlike' onclick='addLike("  + topics[i].novelty[j].noveltyId + ")'> Like <span id='countLike" + topics[i].novelty[j].noveltyId + "' class='badge badge-light'>" + loadLikes(topics[i].novelty[j].noveltyId) + "</span></button>";
                                            topicsHTML += '<button  class="close ml-2" aria-label="Close" onclick="addLike(' + topics[i].novelty[j].noveltyId + ')"> <i class="far fa-thumbs-up"></i>' ;
                                            topicsHTML += '<span id="countLike' + topics[i].novelty[j].noveltyId + '" class="badge badge - light">' + loadLikes(topics[i].novelty[j].noveltyId) + '</span>';
                                            topicsHTML += '</button >';

                                            //кнопки редактирования и удаления новости
                                            if (Role == "admin") {
                                                
                                                topicsHTML += '<button style="outline: none; background: transparent; border: 0;" data-toggle="modal" data-target="#modalChangeNovelty" onclick="editNovelty(' + topics[i].topicId + ',' + topics[i].novelty[j].noveltyId + ')"><i class="fas fa-pen fa-xs"></i></button>';
                                                topicsHTML += '<button class="close ml-2" aria-label="Close" onclick="deleteNovelty(' + topics[i].topicId + ',' + topics[i].novelty[j].noveltyId + ')"> <span aria-hidden="true">&times;</span> </button>';
                                              
                                            }

                                            topicsHTML += '</div>';
                                            topicsHTML += '</div>';                                           
                                                                                      

                                            topicsHTML += '<p class="card-text">' + topics[i].novelty[j].content + "</p>";
                                            topicsHTML += '<hr/>';
                                        }

                                    }
                                    //кнопки доступные только админу и пользователям
                                    if (Role == "admin" || Role == "user") {
                                        topicsHTML += '<button class="btn btn-success btn-block" onclick="setTopic(' + topics[i].topicId + ')" data-toggle="modal" data-target="#createModalNov">Добавить новость</button>';
                                    }
                                    
                                    topicsHTML += '</div></div></div>';
                                    topicsHTML += '</div>';
                                }
                            }
                        }
                        items = topics;
                        document.querySelector("#topicsDiv").innerHTML = topicsHTML;
                    }
                };
                request.send();
            }
        );
}

//создание темы
function createTopic() {

    let nameText = "";
   
    nameText = document.querySelector("#topicnameDiv").value;
    var request = new XMLHttpRequest();
    request.open("POST", uri);
    request.onload = function () {
        // Обработка кода ответа
        var msg = "";
        if (request.status === 401) {
            msg = "У вас не хватает прав для создания";
        } else if (request.status === 201) {
            msg = "Тема добавлена";
            getTopics();
        } else {
            msg = "Неизвестная ошибка";
        }
        document.querySelector("#actionMsg").innerHTML = msg;

        getTopics();
       
        document.querySelector("#topicnameDiv").value = "";
    };
    request.setRequestHeader("Accepts", "application/json;charset=UTF-8");
    request.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    request.send(JSON.stringify({ name: nameText }));
}

//созадние новости
function createNovelty() {
    let titleText = ""; //заголовок
    let contentText = ""; //содержание
    titleText = document.querySelector("#createDivNov").value;
    contentText = document.querySelector("#noveltyContentDiv").value;
    var request = new XMLHttpRequest();
    var request2 = new XMLHttpRequest();

    let topics;
    request.open("GET", uri);
    request.onload = function() {
        topics = JSON.parse(request.responseText);
    }
    request.send();

    request2.open("POST", uri + globalTop + "/novelty");
    request2.onload = function () {
        // Обработка кода ответа
        var msg = "";
        if (request.status === 401) {
            msg = "У вас не хватает прав для создания";
        } else if (request.status === 200) {
            msg = "Новость добавлена";
            getTopics();
        } else {
            msg = "Неизвестная ошибка";
        }
        document.querySelector("#actionMsg").innerHTML = msg;


        getTopics();
        document.querySelector("#createDivNov").value = "";
        document.querySelector("#noveltyContentDiv").value = "";
    };
    request2.setRequestHeader("Accepts", "application/json;charset=UTF-8");
    request2.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    request2.send(JSON.stringify({ title: titleText, content: contentText }));
}

//редактирование темы
function editTopic(id) {
    let elm = document.querySelector("#editDiv");
    elm.style.display = "block";
    if (items) {
        let i;
        for (i in items) {
            if (id === items[i].topicId) {
                document.querySelector("#edit-id").value = items[i].topicId;
                document.querySelector("#edit-name").value = items[i].name;
            }
        }
    }
}

//изменение темы
function udateTopic() {
    const topic = {
        topicid: document.querySelector("#edit-id").value,
      //  url: document.querySelector("#edit-url").value,
        name: document.querySelector("#edit-name").value
    };
    var request = new XMLHttpRequest();
    request.open("PUT", uri + topic.topicid);
    request.onload = function () {
        // Обработка кода ответа
        var msg = "";
        if (request.status === 401) {
            msg = "У вас не хватает прав для изменения";
        } else if (request.status === 204) {
            msg = "Тема изменена";
            getTopics();
        } else {
            msg = "Неизвестная ошибка";
        }
        document.querySelector("#actionMsg").innerHTML = msg;

        getTopics();
        closeInput();
    };
    request.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    request.send(JSON.stringify(topic));
}

//редактирование новости
function editNovelty(topicId, noveltyId) {
    let elm = document.querySelector("#edit2Div");
    elm.style.display = "block";
    if (items) {
        let i;
        for (i in items) {
            if (items[i].topicId === topicId) {
                for (j in items[i].novelty) {
                    if (noveltyId === items[i].novelty[j].noveltyId) {
                        document.querySelector("#edit2-idNovelty").value = noveltyId;
                        document.querySelector("#edit2-idTopic").value = topicId;

                        document.querySelector("#edit2-title").value = items[i].novelty[j].title;
                        document.querySelector("#edit2-content").value = items[i].novelty[j].content;
                    }
                }
            }
        }
    }
}


//изменение новости
function updateNovelty() {
    const novelty = {
        noveltyid: document.querySelector("#edit2-idNovelty").value,
        topicid: document.querySelector("#edit2-idTopic").value,
        title: document.querySelector("#edit2-title").value,
        content: document.querySelector("#edit2-content").value
    };
    var request = new XMLHttpRequest();
    request.open("PUT", uri + novelty.topicid + "/novelties/" + novelty.noveltyid);
    request.onload = function () {
        // Обработка кода ответа
        var msg = "";
        if (request.status === 401) {
            msg = "У вас не хватает прав для изменения";
        } else if (request.status === 204) {
            msg = "Новость изменена";
            getTopics();
        } else {
            msg = "Неизвестная ошибка";
        }
        document.querySelector("#actionMsg").innerHTML = msg;

        getTopics();
        closeInput();
    };
    request.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    request.send(JSON.stringify(novelty));
}

//удаление темы
function deleteTopic(id) {
    let request = new XMLHttpRequest();
    request.open("DELETE", uri + id, false);
    request.onload = function () {
        // Обработка кода ответа
        var msg = "";
        if (request.status === 401) {
            msg = "У вас не хватает прав для удаления";
        } else if (request.status === 204) {
            msg = "Тема удалена";
            getTopics();
        } else {
            msg = "Неизвестная ошибка";
        }
        document.querySelector("#actionMsg").innerHTML = msg;
        getTopics();
    };
    request.send();
}
//удаление новости
function deleteNovelty(Topicid, Noveltyid) {
    let request = new XMLHttpRequest();
    request.open("DELETE", uri + Topicid + "/novelties/" + Noveltyid, false);
    request.onload = function () {
        // Обработка кода ответа
        var msg = "";
        if (request.status === 401) {
            msg = "У вас не хватает прав для удаления";
        } else if (request.status === 204) {
            msg = "Новость удалена";
            getTopics();
        } else {
            msg = "Неизвестная ошибка";
        }
        document.querySelector("#actionMsg").innerHTML = msg;
        getTopics();
    };
    request.send();
}
//закрытие
function closeInput() {
    let elm = document.querySelector("#editDiv");
    elm.style.display = "none";
}
//вход
function logIn() {
    var email, password = "";
    // Считывание данных с формы
    email = document.getElementById("Email").value;
    password = document.getElementById("Password").value;
    var request = new XMLHttpRequest();
    request.open("POST", "/api/Account/Login");
    request.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    request.onreadystatechange = function () {
        // Очистка контейнера вывода сообщений
        document.getElementById("msg").innerHTML = "";
        var mydiv = document.getElementById('formError');
        while (mydiv.firstChild) {
            mydiv.removeChild(mydiv.firstChild);
        }
        // Обработка ответа от сервера
        if (request.responseText !== "") {
            var msg = null;
            msg = JSON.parse(request.responseText);
            document.getElementById("msg").innerHTML = msg.message;
            getTopics();
            // Вывод сообщений об ошибках
            if (typeof msg.error !== "undefined" && msg.error.length >
                0) {
                for (var i = 0; i < msg.error.length; i++) {
                    var ul = document.getElementsByTagName("ul");
                    var li = document.createElement("li");
                    li.appendChild(document.createTextNode(msg.error[i]));
                    ul[0].appendChild(li);
                }
            }
            document.getElementById("Password").value = "";
        }
    };
    // Запрос на сервер
    request.send(JSON.stringify({
        email: email,
        password: password
    }));
}

//выход
function logOff() {
    var request = new XMLHttpRequest();
    request.open("POST", "api/account/logoff");
    request.onload = function () {
        Role = "";
        getTopics();
        var msg = JSON.parse(this.responseText);
        document.getElementById("msg").innerHTML = "";
        getTopics();
        var mydiv = document.getElementById('formError');
        while (mydiv.firstChild) {
            mydiv.removeChild(mydiv.firstChild);
        }
        document.getElementById("msg").innerHTML = msg.message;

    };
    request.setRequestHeader("Content-Type",
        "application/json;charset=UTF-8");
    request.send();

}

//получить текущего юзера
function getCurrentUser() {
    let request = new XMLHttpRequest();
    request.open("POST", "/api/Account/isAuthenticated", true);
    request.onload = function () {
        let myObj = "";
        

        myObj = request.responseText !== "" ?
            JSON.parse(request.responseText) : {};
        document.getElementById("msg").innerHTML = myObj.message;
    };
    request.send();
}

//проверка на админа
function isAdmin() {
    return new Promise(function (resolve, reject) {
        var request = new XMLHttpRequest();
        request.open("POST", "api/account/isAdmin");

        request.onload = function () {
            var response = JSON.parse(this.responseText);
            resolve(response);
        };

        request.onerror = function () {
            reject(new Error("Network Error"));
        };

        request.send();
    });
}
//получить роль пользователя
function GetRole() {
    var request = new XMLHttpRequest();
    request.open("GET", "api/Account/GetRole", false);
    request.onload = function () {
        Role = JSON.parse(request.responseText);
    }
    request.send();
}
//получить айди пользователя
function GetRoleID() {
    var request = new XMLHttpRequest();
    request.open("GET", "api/Account/GetRoleID", false);
    request.onload = function () {
        RoleID = JSON.parse(request.responseText);
    }
    request.send();
}
//загрузить число лайков новости
function loadLikes(id) {
    var request = new XMLHttpRequest();
    request.open("GET", urilike + id, false);
    request.onload = function () {
        countlike = request.responseText;
    };
    request.send();
    return countlike;
}
//добавить лайк новости
function addLike( id) {
    //getCurrentUser();
    GetRoleID();
    var novelty = id;
    var user = RoleID;
    var request = new XMLHttpRequest();
    request.open("POST", urilike);
    request.setRequestHeader("Content-Type", "application/json; charset=UTF-8");
    request.onload = function () {
        getTopics();
    };
    request.send(JSON.stringify({
        noveltyId: novelty,
        userId: user
    }));
}

//обновление страницы
function refreshPage() {
    window.location.reload();
} 


