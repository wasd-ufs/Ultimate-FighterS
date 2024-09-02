using UnityEngine;
using UnityEngine.Events;

public class GlobalEvents
{
    /*
    Adicionar atributos UnityEvent conforme a necessidade.
    Para cada novo atributo UnityEvent deve-se criar uma nova 
    classe que herda de Notifier e implemementa o método Notify().
    Caso precise use OnPLayerDiedNotify como exemplo. 
     */
    public static UnityEvent<GameObject> onPlayerDied = new();
}
