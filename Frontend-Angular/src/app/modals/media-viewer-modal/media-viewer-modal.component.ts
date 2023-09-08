import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-media-viewer-modal',
  templateUrl: './media-viewer-modal.component.html',
  styleUrls: ['./media-viewer-modal.component.css']
})
export class MediaViewerModalComponent {
  @Input() mediaUrl!: string;
  @Input() mediaName!: string;
  @Input() mediaType!: string;
  @Output() closeEvent = new EventEmitter<void>();

  close() {
    this.closeEvent.emit();
  }
}