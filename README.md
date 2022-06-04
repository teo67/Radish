<h1>RADISH</h1>
<p>{ like the vegetable. }</p>
<br>
<h2>So what's it all about, anyway?</h2>
<p>Radish is an easy-to-learn, object-oriented programming language written in C#. If you know JavaScript, you know Radish. However, Radish has a few of its own quirks to keep in mind (don't ask why these exist)
    <ul>
        <li>To declare a variable, use "dig" or just "d"<br><em>Example: make name</em></li>
        <li>To set the value of a variable, use "plant" or the plain old = symbol<br><em>Example: make name set "Theo"</em></li>
        <li>Radish currently supports only multiline comments, just start and end your comment with #<br><em>Example: # this is a comment #</em><br>As a side note, Radish is completely linebreak blind. That's right, no semicolons. You can write as many statements on the same line as you want!</li>
        <li>Functions are declared using either "tool" or "t"<br><em>Example: make sayHi set f(name) {
            output(name)
        }</em><br>Oh yeah, forgot to mention that the print function is called output.</li>
        <li>There are a few other details, but you'll get used to them along the way.</li>
    </ul>
In terms of OOP, perhaps a demonstration would be the best way to explain. See if you can figure out what this code does:<br><em>
dig FunkyList plant class {<br>
    &emsp;dig stored<br>
    &emsp;dig constructor plant tool(arr) {<br>
        &emsp;&emsp;this.stored plant arr<br>
    &emsp;}<br>
    &emsp;dig get plant tool(index) {<br>
        &emsp;&emsp;if(index % 2 == 1) { # for odd numbered requests #<br>
            &emsp;&emsp;&emsp;harvest "no list element for you"<br>
        &emsp;&emsp;}<br>
        &emsp;&emsp;harvest this.stored[index]<br>
    &emsp;}<br>
    &emsp;dig push plant tool(val) {<br>
        &emsp;&emsp;this.stored.push(val)<br>
    &emsp;}<br>
}<br>
<br>
dig li plant new FunkyList([1, 2, 3])<br>
holler(li.get(0)) # output = 1 #<br>
holler(li.get(1)) # output = no list element for you #<br>
holler(li.get(2)) # output = 3 #<br>
li.push("list element please")<br>
holler(li.get(3)) # output = no list element for you #<br>
holler(li.get(4)) # error #<br>
</em><br>Good luck on your journey with Radish!</p>
<br>
<p>TODO<br><ol>
    <li>Add syntax for class to class inheritance // DONE</li>
    <li>Add syntax to call inherited constructor // DONE</li>
    <li>Add more prototype class functions, such as array.pop() // DONE (sort of)</li>
    <li>Introduce getter functions, so that array.length can be called instead of array.length() // DONE</li>
    <li>Add tags for object/class properties, such as static, public/private/protected, etc // DONE</li>
    <li>Farm theme! // DONE</li>
    <li>Better debugging! Literally it's so bad // DONE</li>
    <li>Add manual exception throwing + try/catch syntax // DONE</li>
    <li>Make VSCode extension so that the code doesn't look so bland (also cool file symbol) // ALMOST DONE</li>
    <li>Add importing/exporting from other files!!! // DONE</li>
    <li>Package it so that users can install radish and run files without seeing all of the machinery // DONE</li>
    <li>async??</li>
</ol></p>
