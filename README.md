# TaskManager_With_Sqlite
La Task Manager Application es una aplicación de escritorio desarrollada en WPF (Windows Presentation Foundation), .Net8 y los patrones de MVVM y DAO, diseñada para ayudar a los usuarios a gestionar sus tareas diarias de manera eficiente. La aplicación permite a los usuarios crear, editar, eliminar y categorizar tareas, así como marcar tareas como completas o en progreso. Los datos se almacenan de manera persistente utilizando SQLite, y la interacción con la base de datos se maneja a través de Entity Framework, proporcionando una solución robusta y escalable.

**Funcionalidades**

Crear Editar Buscar y Eliminar Tareas
Registrar y editar usuarios.

**Tecnologias Utilizadas**

.Net8 - WPF(Windows Presentation Fundation) - EntityFramework - SQlite.

# Estructura del proyecto

**AppResource:** Contiene estilos de componentes personalizados.

**Images:** Contiene las imagenes que se han utilizado en la UI.

**Interfaces:** Contiene la interfaz con los metodos abstractos que sera implementada por las clases DAO.

**MaterialDesignTheme** Contiene los diccionarios de recursos de la libreria "MaterialDesignTheme" utilizada para hacer mas atractiva la UI.

**Model\DAO:** Contiene las clases DAO que implementan la interfaz "ITaskManagerServiceDAO" cuyos metodos se encargan de la manipulacion de la base de datos.

**Model\DbContext:** Contiene las clases modelo que serviran como plantilla para implementar el patron DAO ademas de la clase "TaskManagerDbContext" que posee el modelado de la base de datos SQlite.

**View:** Contiene los archivos XAML de la (UI) para la presentacion eh interaccion de los datos.

**ViewModel:** Contiene las clases que manejan la logica de manipulacion y presentacion de datos para ser presentados por las vistas.

# Diagrama de relacion

<img src="https://github.com/DanielGerardoHC/TaskManager_With_SQlite/blob/main/TaskManagerDB-Diagram.jpeg" title="TaskManagerDB" alt="TaskManagerDB" width="400" height="355"/> <img>





