using UnityEngine;
public class LinkedList<T> 
{
    private Node first, last;
    public int Count { get; private set; }

    public void AddFirst(T item)
    {
        Node n = new Node();
        n.item = item;
        if (last is null)
        {
            last = first = n;
        }
        else
        {
            first.previous = n;
            n.next = first;
            first = n;
        }
    }
    public void Remove(T item) {

        // search the list until you find a node that has the same item
        // remove this node from the list
        // this will be either removefirst, remove last, or removing an element from the middle

        Node current = first;
        Node prev = first;

        while (current is not null)
        {
            if (current.item.Equals(item))
            {
                // remove the item
                first = current.next;
            }
            current = current.next;
        }
    }


    private Node current;
    public T Reset()
    {
        current = first;
        return current.item;
    }
    public T MoveNext() {
        current = current.next;
        return current.item;
    }
    public bool HasNext()
    {
        return current.next is not null;
    }

    class Node
    {
        public T item;
        public Node next;
        public Node previous;
    }
}

public class BoidTest : MonoBehaviour
{
    [SerializeField] private float alignmentForce = 0.1f;
    [SerializeField] private float cohesionForce = 0.1f;
    [SerializeField] private float separationForce = 0.1f;
    [SerializeField] private int speed = 1;

    private Vector2 velocity;

    private LinkedList<BoidTest> closestBoids;

    void Start()
    {
        velocity = (Random.onUnitSphere * 10);
        closestBoids = new LinkedList<BoidTest>();
    }

    void Update()
    {
        Alignment();
        Cohesion();
        Separation();
        transform.position += (Vector3)(velocity * Time.deltaTime);
        velocity = Vector2.ClampMagnitude(velocity, speed);
    }

    void Alignment()
    {
        var alignmentVector = new Vector2();
        for (var boid = closestBoids.Reset(); closestBoids.HasNext(); closestBoids.MoveNext())
        {
            var direction = boid.velocity;
            alignmentVector += direction;
        }

        alignmentVector.Normalize();
        velocity += alignmentVector * alignmentForce;
    }

    void Cohesion()
    {
        var cohesionPoint = new Vector2();
        for (var boid = closestBoids.Reset(); closestBoids.HasNext(); closestBoids.MoveNext())
        {
            cohesionPoint += (Vector2) boid.transform.position;
        }

        cohesionPoint /= closestBoids.Count;

        var directionVector = cohesionPoint - (Vector2) transform.position;
        velocity += directionVector.normalized * cohesionForce;
    }

    void Separation()
    {
        var separationVector = new Vector2();
        for (var boid = closestBoids.Reset(); closestBoids.HasNext(); closestBoids.MoveNext())
        {
            var direction = this.transform.position - boid.transform.position;
            separationVector += (Vector2) direction;
        }

        separationVector.Normalize();
        velocity += separationVector * separationForce;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var boid = collision.GetComponent<BoidTest>();
        if (boid != null)
        {
            closestBoids.AddFirst(boid);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var boid = collision.GetComponent<BoidTest>();
        if (boid != null)
        {
            closestBoids.Remove(boid);
        }
    }
}