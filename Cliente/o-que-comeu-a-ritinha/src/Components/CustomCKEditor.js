import { useState, useEffect, useRef } from 'react';
import { CKEditor } from '@ckeditor/ckeditor5-react';
import {
    ClassicEditor,
    AccessibilityHelp,
    AutoImage,
    AutoLink,
    Autosave,
    BlockQuote,
    Bold,
    CloudServices,
    Essentials,
    Heading,
    ImageBlock,
    ImageInsertViaUrl,
    ImageToolbar,
    ImageUpload,
    Indent,
    Italic,
    Link,
    List,
    MediaEmbed,
    Paragraph,
    SelectAll,
    SpecialCharacters,
    Table,
    TableToolbar,
    Undo
} from 'ckeditor5';

import 'ckeditor5/ckeditor5.css';

export default function CustomCKEditor({ data, onChange }) {
    const editorContainerRef = useRef(null);
    const editorRef = useRef(null);
    const [isLayoutReady, setIsLayoutReady] = useState(false);

    useEffect(() => {
        setIsLayoutReady(true);

        return () => setIsLayoutReady(false);
    }, []);

    const editorConfig = {
        toolbar: {
            items: [
                'heading',
                '|',
                'bold',
                'italic',
                'link',
                'bulletedList',
                'numberedList',
                '|',
                'outdent',
                'indent',
                '|',
                'blockQuote',
                'insertTable',
                'mediaEmbed',
                'undo',
                'redo'
            ],
            shouldNotGroupWhenFull: true
        },
        plugins: [
            AccessibilityHelp,
            AutoImage,
            AutoLink,
            Autosave,
            BlockQuote,
            Bold,
            CloudServices,
            Essentials,
            Heading,
            ImageBlock,
            ImageInsertViaUrl,
            ImageToolbar,
            ImageUpload,
            Indent,
            Italic,
            Link,
            List,
            MediaEmbed,
            Paragraph,
            SelectAll,
            SpecialCharacters,
            Table,
            TableToolbar,
            Undo
        ],
        heading: {
            options: [
                { model: 'paragraph', title: 'Paragraph', class: 'ck-heading_paragraph' },
                { model: 'heading1', view: 'h2', title: 'Heading 1', class: 'ck-heading_heading1' },
                { model: 'heading2', view: 'h3', title: 'Heading 2', class: 'ck-heading_heading2' },
                { model: 'heading3', view: 'h4', title: 'Heading 3', class: 'ck-heading_heading3' }
            ]
        },
        image: {
            toolbar: ['imageTextAlternative']
        },
        link: {
            addTargetToExternalLinks: true,
            defaultProtocol: 'https://',
        },
        table: {
            contentToolbar: ['tableColumn', 'tableRow', 'mergeTableCells']
        },
        isInline: true,
    };    

    return (
        <div>
            <div className="main-container">
                <div className="editor-container editor-container_classic-editor" ref={editorContainerRef}>
                    <div className="editor-container__editor">
                        <div ref={editorRef}>
                            {isLayoutReady && (
                                <CKEditor
                                    editor={ClassicEditor}
                                    config={editorConfig}
                                    data={data}
                                    onChange={(event, editor) => {
                                        const data = editor.getData();
                                        onChange(data); // Emit the updated data
                                    }}
                                />
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}