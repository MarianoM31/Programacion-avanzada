# AhorcadoMVC – Proyecto Grupo #2

# Integrantes

Nombre                               Carné            Usuario GIT                          Correo GIT
Jazmin Pamela Montenegro Baltodano | FI23032284   |   JazminPamelaMontenegroBaltodano   |  jmontenegro00328@ufide.ac.cr
Mariano Mora Arrieta               | FI23032359   |   MarianoM31                        |  marianomora31@hotmail.com
Argenis David Cerrato Amador       | FH23015629   |   imDavid64                         |  davidamador0999@gmail.com
Nelson Rodriguez lopez             | FI20016869   |   nelson2587                        |  nrodriguez087@gmail.com

# Diagrama (Mermaid)
### Diagrama (Mermaid)

```mermaid
erDiagram
  JUGADORES ||--o{ PARTIDAS : "tiene"
  NIVELES   ||--o{ PARTIDAS : "configura"
  PALABRAS  ||--o{ PARTIDAS : "usa"
  JUGADORES ||--o{ VISTA_ESCALAFON : "agrega (vista)"

  JUGADORES {
    INT id_jugador PK "PK (no IDENTITY)"
    NVARCHAR(100) nombre "NOT NULL"
  }

  NIVELES {
    INT id_nivel PK
    NVARCHAR(20) nombre_nivel "UNIQUE, NOT NULL"
    INT tiempo_segundos "NOT NULL"
  }

  PALABRAS {
    INT id_palabra PK "IDENTITY(1,1)"
    NVARCHAR(20) palabra "UNIQUE, NOT NULL, CHECK LEN 5..10"
  }

  PARTIDAS {
    INT id_partida PK "IDENTITY(1,1)"
    INT id_jugador FK "-> JUGADORES.id_jugador, NOT NULL"
    INT id_nivel   FK "-> NIVELES.id_nivel, NOT NULL"
    INT id_palabra FK "-> PALABRAS.id_palabra, NOT NULL"
    DATETIME fecha "DEFAULT GETDATE(), NOT NULL"
    VARCHAR(10) resultado "CHECK ('Ganada','Perdida')"
  }

  VISTA_ESCALAFON {
    INT id_jugador "de JUGADORES"
    NVARCHAR(100) nombre
    INT Marcador
    INT Ganadas
    INT Perdidas
  }
```



# Prompts de IA usados

Prompts: Necesito un partial para mostrar el escalafón ordenado por Marcador
Respuesta: Crea RankingItemViewModel y una acción HomeController.Ranking() que consuma Vista_Escalafon

# Sitios Consultados

- Chatgpt
- Google 
- https://www.youtube.com/watch?v=C4kahP-ucT0&t=1s
- https://gist.github.com/keraf/b3d5bacc7be1d4e681bfbac91722957e
- https://github.blog/developer-skills/github/include-diagrams-markdown-files-mermaid/


