<h1>RADISH</h1>
<h3>Real Awesome Dynamic Interface w/ Structural Happiness</h3>
<p>Just kidding, it's just Radish like the vegetable.</p>
<br>
<h2>So what's it all about, anyway?</h2>
<p>Radish is an easy-to-learn, object-oriented programming language written in C#. If you know JavaScript, you know Radish. However, Radish has a few of its own quirks to keep in mind (don't ask why these exist)
    <ul>
        <li>To declare a variable, use "make" or just "m"<br><strong>Example: make name</strong></li>
        <li>To set the value of a variable, use "set" or the plain old = symbol<br><strong>Example: make name set "Theo"</strong></li>
        <li>Radish currently supports only multiline comments, just start and end your comment with #<br><strong>Example: # this is a comment #<strong><br>As a side note, Radish is completely linebreak blind. That's right, no semicolons. You can write as many statements on the same line as you want!</li>
        <li>Functions are declared using either "function" or "f"<br><strong>Example: make sayHi set f(name) {
            output(name)
        }</strong><br>Oh yeah, forgot to mention that the print function is called output.</li>
        <li>There are a few other details, but you'll get used to them along the way.</li>
    </ul>
In terms of OOP, perhaps a demonstration would be the best way to explain. See if you can figure out what this code does:<br><strong>
make FunkyList set class {<br>
    make stored<br>
    make constructor set f(arr) {<br>
        this.stored set arr<br>
    }<br>
    make get set f(index) {<br>
        if(index % 2 == 1) { # for odd numbered requests #<br>
            out "no list element for you"<br>
        }<br>
        out this.stored[index]<br>
    }<br>
    make push set f(val) {<br>
        this.stored.push(val)<br>
    }<br>
}<br>
<br>
make li set new FunkyList([1, 2, 3])<br>
output(li.get(0)) # output = 1 #<br>
output(li.get(1)) # output = no list element for you #<br>
output(li.get(2)) # output = 3 #<br>
li.push("list element please")<br>
output(li.get(3)) # output = no list element for you #<br>
output(li.get(4)) # error #<br>
</strong><br>Good luck on your journey with Radish!</p>
