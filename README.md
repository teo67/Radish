<h1>RADISH</h1>
<h3>Real Awesome Dynamic Interface w/ Structural Happiness</h3>
<p>Just kidding, it's just Radish like the vegetable.</p>
<br>
<h2>So what's it all about, anyway?</h2>
<p>Radish is an easy-to-learn, object-oriented programming language written in C#. If you know JavaScript, you know Radish. However, Radish has a few of its own quirks to keep in mind (don't ask why these exist)
    <ul>
        <li>To declare a variable, use "make" or just "m"<br><em>Example: make name</em></li>
        <li>To set the value of a variable, use "set" or the plain old = symbol<br><em>Example: make name set "Theo"</em></li>
        <li>Radish currently supports only multiline comments, just start and end your comment with #<br><em>Example: # this is a comment #</em><br>As a side note, Radish is completely linebreak blind. That's right, no semicolons. You can write as many statements on the same line as you want!</li>
        <li>Functions are declared using either "function" or "f"<br><em>Example: make sayHi set f(name) {
            output(name)
        }</em><br>Oh yeah, forgot to mention that the print function is called output.</li>
        <li>There are a few other details, but you'll get used to them along the way.</li>
    </ul>
In terms of OOP, perhaps a demonstration would be the best way to explain. See if you can figure out what this code does:<br><em>
make FunkyList set class {<br>
    &emsp;make stored<br>
    &emsp;make constructor set f(arr) {<br>
        &emsp;&emsp;this.stored set arr<br>
    &emsp;}<br>
    &emsp;make get set f(index) {<br>
        &emsp;&emsp;if(index % 2 == 1) { # for odd numbered requests #<br>
            &emsp;&emsp;&emsp;out "no list element for you"<br>
        &emsp;&emsp;}<br>
        &emsp;&emsp;out this.stored[index]<br>
    &emsp;}<br>
    &emsp;make push set f(val) {<br>
        &emsp;&emsp;this.stored.push(val)<br>
    &emsp;}<br>
}<br>
<br>
make li set new FunkyList([1, 2, 3])<br>
output(li.get(0)) # output = 1 #<br>
output(li.get(1)) # output = no list element for you #<br>
output(li.get(2)) # output = 3 #<br>
li.push("list element please")<br>
output(li.get(3)) # output = no list element for you #<br>
output(li.get(4)) # error #<br>
</em><br>Good luck on your journey with Radish!</p>
<br>
<p>TODO<br><ol>
    <li>Add syntax for class to class inheritance</li>
    <li>Add syntax to call inherited constructor</li>
    <li>Add more prototype class functions, such as array.pop()</li>
    <li>Introduce getter functions, so that array.length can be called instead of array.length()</li>
    <li>Add tags for object/class properties, such as static, public/private/protected, etc</li>
    <li>Better debugging! Literally it's so bad</li>
    <li>Make VSCode extension so that the code doesn't look so bland (also cool file symbol)</li>
    <li>Add importing/exporting from other files!!!</li>
    <li>Package it so that users can install radish and run files without seeing all of the machinery</li>
</ol></p>